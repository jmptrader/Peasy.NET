﻿using Peasy.Core;
using Peasy.Core.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.ComponentModel.DataAnnotations;

namespace Peasy.Tests.Rules
{
    [TestClass]
    public class ServiceCommandTests
    {
        private static string _count = string.Empty;

        [TestInitialize]
        public void Initialize()
        {
            _count = string.Empty;
        }

        private Action initializationMethod = () =>
        {
            _count += "1";
        };

        private Func<Task> initializationAsyncMethod = () =>
        {
            _count += "1";
            return Task.FromResult<object>(null);
        };

        private Func<IEnumerable<ValidationResult>> getErrorsMethod = () =>
        {
            _count += "2";
            return Enumerable.Empty<ValidationResult>();
        };

        private Func<Task<IEnumerable<ValidationResult>>> getErrorsAsyncMethod = () =>
        {
            _count += "2";
            return Task.FromResult(Enumerable.Empty<ValidationResult>());
        };

        private Func<IEnumerable<IRule>> getBusinessRulesMethod = () =>
        {
            _count += "3";
            return Enumerable.Empty<IRule>();
        };

        private Func<Task<IEnumerable<IRule>>> getBusinessRulesAsyncMethod = () =>
        {
            _count += "3";
            return Task.FromResult(Enumerable.Empty<IRule>());
        };

        private Action executeMethod = () =>
        {
            _count += "4";
        };

        private Func<Task> executeAsyncMethod = () =>
        {
            _count += "4";
            return Task.FromResult<object>(null);
        };

        [TestMethod]
        public async Task ServiceCommand_Composition_1()
        {
            var command = new ServiceCommand
            (
                initializationMethod,
                initializationAsyncMethod,
                getErrorsMethod,
                getErrorsAsyncMethod,
                executeMethod,
                executeAsyncMethod
            );

            command.Execute();
            await command.ExecuteAsync();

            _count.ShouldBe("124124");
        }

        [TestMethod]
        public async Task ServiceCommand_Composition_2()
        {

            var command = new ServiceCommand
            (
                initializationMethod,
                initializationAsyncMethod,
                executeMethod,
                executeAsyncMethod
            );

            command.Execute();
            await command.ExecuteAsync();

            _count.ShouldBe("1414");
        }

        [TestMethod]
        public async Task ServiceCommand_Composition_3()
        {
            var command = new ServiceCommand
            (
                initializationMethod,
                executeMethod
            );

            command.Execute();
            await command.ExecuteAsync();

            _count.ShouldBe("14");
        }

        [TestMethod]
        public async Task ServiceCommand_Composition_4()
        {
            var command = new ServiceCommand
            (
                initializationAsyncMethod,
                executeAsyncMethod
            );

            command.Execute();
            await command.ExecuteAsync();

            _count.ShouldBe("14");
        }

        [TestMethod]
        public async Task ServiceCommand_Composition_5()
        {
            var command = new ServiceCommand
            (
                getErrorsMethod,
                getErrorsAsyncMethod,
                executeMethod,
                executeAsyncMethod
            );

            command.Execute();
            await command.ExecuteAsync();

            _count.ShouldBe("2424");
        }

        [TestMethod]
        public async Task ServiceCommand_Composition_6()
        {
            var command = new ServiceCommand
            (
                executeMethod,
                executeAsyncMethod
            );

            command.Execute();
            await command.ExecuteAsync();

            _count.ShouldBe("44");
        }

        [TestMethod]
        public async Task ServiceCommand_Composition_7()
        {
            var command = new ServiceCommand
            (
                executeMethod,
                getErrorsMethod
            );

            command.Execute();
            await command.ExecuteAsync();

            _count.ShouldBe("24");
        }

        [TestMethod]
        public async Task ServiceCommand_Composition_8()
        {
            var command = new ServiceCommand
            (
                executeAsyncMethod,
                getErrorsAsyncMethod
            );

            command.Execute();
            await command.ExecuteAsync();

            _count.ShouldBe("24");
        }

        [TestMethod]
        public async Task ServiceCommand_Composition_9()
        {
            var command = new ServiceCommand
            (
                executeMethod,
                executeAsyncMethod,
                getBusinessRulesMethod,
                getBusinessRulesAsyncMethod
            );

            command.Execute();
            await command.ExecuteAsync();

            _count.ShouldBe("3434");
        }

        [TestMethod]
        public async Task ServiceCommand_Composition_10()
        {
            var command = new ServiceCommand
            (
                executeMethod,
                getBusinessRulesMethod
            );

            command.Execute();
            await command.ExecuteAsync();

            _count.ShouldBe("34");
        }

        [TestMethod]
        public async Task ServiceCommand_Composition_11()
        {
            var command = new ServiceCommand
            (
                executeAsyncMethod
            );

            command.Execute();
            await command.ExecuteAsync();

            _count.ShouldBe("4");
        }

        [TestMethod]
        public async Task ServiceCommand_Composition_12()
        {
            var command = new ServiceCommand
            (
                executeAsyncMethod,
                getBusinessRulesAsyncMethod
            );

            command.Execute();
            await command.ExecuteAsync();

            _count.ShouldBe("34");
        }

        [TestMethod]
        public async Task ServiceCommand_Composition_13()
        {
            var command = new ServiceCommand
            (
                initializationAsyncMethod,
                executeAsyncMethod,
                getBusinessRulesAsyncMethod
            );

            command.Execute();
            await command.ExecuteAsync();

            _count.ShouldBe("134");
        }

        [TestMethod]
        public async Task ServiceCommand_Composition_14()
        {
            var command = new ServiceCommand
            (
                initializationAsyncMethod,
                executeAsyncMethod,
                getErrorsAsyncMethod
            );

            command.Execute();
            await command.ExecuteAsync();

            _count.ShouldBe("124");
        }
    }
}
