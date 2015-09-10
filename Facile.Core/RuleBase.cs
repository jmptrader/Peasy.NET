﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Facile.Core
{
    /// <summary>
    /// A validation rule to run againt records being processed.
    /// </summary>
    public abstract class RuleBase : IRule
    {
        /// <summary>
        /// The action to perform once when this rule passes validation.
        /// </summary>
        protected Action<IRule> _ifValidThenExecute;

        /// <summary>
        /// The action to perform once when this rule fails validation.
        /// </summary>
        protected Action<IRule> _ifInvalidThenExecute;

        /// <summary>
        /// Gets or sets the message to be supplied to caller in the event that no rule dependencies exist via IfValidThenValidate()
        /// </summary>
        public string ErrorMessage { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether this rule is valid.
        /// </summary>
        /// <value>
        /// <c>True</c> if this instance is valid; otherwise, <c>false</c>.
        /// </value>
        public bool IsValid { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether this rule is valid.
        /// </summary>
        /// <value>
        /// <c>True</c> if this instance is invalid; otherwise, <c>false</c>.
        /// </value>
        public bool IsInvalid
        {
            get { return !IsValid; }
        }

        /// <summary>
        /// Gets or sets the <see cref="RuleBase"/> that should be evaluated after this instance.
        /// </summary>
        /// <value>
        /// The successor <see cref="RuleBase"/>.
        /// </value>
        private List<IRule[]> Successor = new List<IRule[]>();

        /// <summary>
        /// Validates this rule.
        /// </summary>
        public IRule Validate()
        {
            IsValid = true;
            OnValidate();
            if (IsValid)
            {
                if (Successor != null)
                {
                    foreach (var ruleList in Successor)
                    {
                        foreach (var rule in ruleList)
                        {
                            rule.Validate();
                            if (!rule.IsValid)
                            {
                                IsValid = rule.IsValid;
                                ErrorMessage = rule.ErrorMessage;
                                HandleIfInvalidThenExecute();
                                break; // early exit, don't bother further rule execution
                            }
                        }
                        if (!IsValid) break;
                    }
                }
                HandleIfValidThenExecute();
            }
            HandleIfInvalidThenExecute();
            return this;
        }

        private void HandleIfValidThenExecute()
        {
            if (_ifValidThenExecute != null)
            {
                _ifValidThenExecute(this);
                _ifValidThenExecute = null;
            }
        }

        private void HandleIfInvalidThenExecute()
        {
            if (_ifInvalidThenExecute != null)
            {
                _ifInvalidThenExecute(this);
                _ifInvalidThenExecute = null;
            }
        }

        /// <summary>
        /// Validates the supplied <see cref="RuleBase"/> if this rule passes validation.
        /// </summary>
        /// <param name="rule">The <see cref="RuleBase"/>.</param>
        /// <returns>The supplied <see cref="RuleBase"/>.</returns>
        public IRule IfValidThenValidate(params IRule[] rules)
        {
            Successor.Add(rules);
            return this;
        }

        /// <summary>
        /// Execute an action if the rule passes validation.
        /// </summary>
        /// <param name="method">The action to perform.</param>
        public IRule IfValidThenExecute(Action<IRule> method)
        {
            _ifValidThenExecute = method;
            return this;
        }

        /// <summary>
        /// Execute an action if the rule fails validation.
        /// </summary>
        /// <param name="method">The action to perform.</param>
        public IRule IfInvalidThenExecute(Action<IRule> method)
        {
            _ifInvalidThenExecute = method;
            return this;
        }

        /// <summary>
        /// Called when the <see cref="M:Facile.Rules.RuleBase.Validate()"/> method is called.
        /// </summary>
        /// <returns>
        /// <c>True</c> if validation succeeded; otherwise <c>false</c>.
        /// </returns>
        protected abstract void OnValidate();

        protected virtual void Invalidate(string errorMessage)
        {
            ErrorMessage = errorMessage;
            IsValid = false;
        }
    }
}
