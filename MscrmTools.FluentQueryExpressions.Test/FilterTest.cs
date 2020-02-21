using Microsoft.Xrm.Sdk.Query;
using MscrmTools.FluentQueryExpressions.Test.AppCode;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace MscrmTools.FluentQueryExpressions.Test
{
    public class FilterTest
    {
        [Fact]
        public void ShouldAddFilter()
        {
            var query = new Query<Account>()
                .AddFilters(LogicalOperator.Or, new Filter(LogicalOperator.Or).AddFilters(new Filter(LogicalOperator.Or)));

            Assert.Equal(LogicalOperator.Or, query.QueryExpression.Criteria.FilterOperator);
            Assert.Single(query.QueryExpression.Criteria.Filters);
            Assert.Equal(LogicalOperator.Or, query.QueryExpression.Criteria.Filters.First().Filters.First().FilterOperator);
        }

        [Fact]
        public void ShouldAddFilterWithoutOperator()
        {
            var query = new Query<Account>()
                .AddFilters(new Filter()
                    .AddFilters(new Filter()));

            Assert.Equal(LogicalOperator.And, query.QueryExpression.Criteria.FilterOperator);
            Assert.Single(query.QueryExpression.Criteria.Filters);
            Assert.Equal(LogicalOperator.And, query.QueryExpression.Criteria.Filters.First().FilterOperator);
            Assert.Equal(LogicalOperator.And, query.QueryExpression.Criteria.Filters.First().Filters.First().FilterOperator);
        }

        [Fact]
        public void ShouldCreateFilter()
        {
            var query = new Query<Account>()
                .AddFilters(new Filter()
                    .AddFilters(new Filter()));

            Assert.Equal(LogicalOperator.And, query.QueryExpression.Criteria.FilterOperator);
            Assert.Single(query.QueryExpression.Criteria.Filters);
            Assert.Equal(LogicalOperator.And, query.QueryExpression.Criteria.Filters.First().FilterOperator);
        }

        [Fact]
        public void ShouldCreateFilterWithOr()
        {
            var query = new Query<Account>()
                .AddFilters(new Filter(LogicalOperator.Or));

            Assert.Single(query.QueryExpression.Criteria.Filters);
            Assert.Equal(LogicalOperator.Or, query.QueryExpression.Criteria.Filters.First().FilterOperator);
        }

        [Fact]
        public void ShouldSetLogicalOperatorOr()
        {
            var query = new Query<Account>()
                .AddFilters(new Filter(LogicalOperator.Or)
                    .AddFilters(new Filter()
                        .SetDefaultFilterOperator(LogicalOperator.Or)));

            Assert.Equal(LogicalOperator.Or, query.QueryExpression.Criteria.Filters.First().Filters.First().FilterOperator);
        }

        #region Conditions

        [Fact]
        public void ShouldSetWhere()
        {
            var guid = Guid.NewGuid();
            var query = new Query<Account>()
                .AddFilters(new Filter().Where(Account.Fields.AccountId, ConditionOperator.Above, guid));

            Assert.Equal(Account.Fields.AccountId, query.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.Above, query.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal(guid, query.QueryExpression.Criteria.Filters.First().Conditions.First().Values.Single());

            var query2 = new Query<Account>()
                .AddFilters(new Filter().Where(Account.EntityLogicalName, Account.Fields.AccountId, ConditionOperator.Above, guid));

            Assert.Equal(Account.EntityLogicalName, query2.QueryExpression.Criteria.Filters.First().Conditions.First().EntityName);
            Assert.Equal(Account.Fields.AccountId, query2.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.Above, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal(guid, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values.Single());
        }

        [Fact]
        public void ShouldSetWhereAbove()
        {
            var guid = Guid.NewGuid();
            var query = new Query<Account>()
                .AddFilters(new Filter().WhereAbove(Account.Fields.AccountId, guid));

            Assert.Equal(Account.Fields.AccountId, query.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.Above, query.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal(guid, query.QueryExpression.Criteria.Filters.First().Conditions.First().Values.Single());

            var query2 = new Query<Account>()
                .AddFilters(new Filter().WhereAbove(Account.Fields.AccountId, guid, Account.EntityLogicalName));

            Assert.Equal(Account.EntityLogicalName, query2.QueryExpression.Criteria.Filters.First().Conditions.First().EntityName);
            Assert.Equal(Account.Fields.AccountId, query2.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.Above, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal(guid, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values.Single());
        }

        [Fact]
        public void ShouldSetWhereAboveOrEqual()
        {
            var guid = Guid.NewGuid();
            var query = new Query<Account>()
                .AddFilters(new Filter().WhereAboveOrEqual(Account.Fields.AccountId, guid));

            Assert.Equal(Account.Fields.AccountId, query.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.AboveOrEqual, query.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal(guid, query.QueryExpression.Criteria.Filters.First().Conditions.First().Values.Single());

            var query2 = new Query<Account>()
                .AddFilters(new Filter().WhereAboveOrEqual(Account.Fields.AccountId, guid, Account.EntityLogicalName));

            Assert.Equal(Account.EntityLogicalName, query2.QueryExpression.Criteria.Filters.First().Conditions.First().EntityName);
            Assert.Equal(Account.Fields.AccountId, query2.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.AboveOrEqual, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal(guid, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values.Single());
        }

        [Fact]
        public void ShouldSetWhereBeginsWith()
        {
            var query = new Query<Account>()
                .AddFilters(new Filter().WhereBeginsWith(Account.Fields.Name, "test"));

            Assert.Equal(Account.Fields.Name, query.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.BeginsWith, query.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal("test", query.QueryExpression.Criteria.Filters.First().Conditions.First().Values.Single());

            var query2 = new Query<Account>()
                .AddFilters(new Filter().WhereBeginsWith(Account.Fields.Name, "test", Account.EntityLogicalName));

            Assert.Equal(Account.EntityLogicalName, query2.QueryExpression.Criteria.Filters.First().Conditions.First().EntityName);
            Assert.Equal(Account.Fields.Name, query2.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.BeginsWith, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal("test", query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values.Single());
        }

        [Fact]
        public void ShouldSetWhereBetween()
        {
            var query = new Query<Account>()
                .AddFilters(new Filter().WhereBetween(Account.Fields.NumberOfEmployees, 10, 50));

            Assert.Equal(Account.Fields.NumberOfEmployees, query.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.Between, query.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal(10, query.QueryExpression.Criteria.Filters.First().Conditions.First().Values[0]);
            Assert.Equal(50, query.QueryExpression.Criteria.Filters.First().Conditions.First().Values[1]);

            var query2 = new Query<Account>()
                .AddFilters(new Filter().WhereBetween(Account.Fields.NumberOfEmployees, 10, 50, Account.EntityLogicalName));

            Assert.Equal(Account.EntityLogicalName, query2.QueryExpression.Criteria.Filters.First().Conditions.First().EntityName);
            Assert.Equal(Account.Fields.NumberOfEmployees, query2.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.Between, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal(10, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values[0]);
            Assert.Equal(50, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values[1]);
        }

        [Fact]
        public void ShouldSetWhereChildOf()
        {
            var guid = Guid.NewGuid();
            var query = new Query<Account>()
                .AddFilters(new Filter().WhereChildOf(Account.Fields.AccountId, guid));

            Assert.Equal(Account.Fields.AccountId, query.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.ChildOf, query.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal(guid, query.QueryExpression.Criteria.Filters.First().Conditions.First().Values.Single());

            var query2 = new Query<Account>()
                .AddFilters(new Filter().WhereChildOf(Account.Fields.AccountId, guid, Account.EntityLogicalName));

            Assert.Equal(Account.EntityLogicalName, query2.QueryExpression.Criteria.Filters.First().Conditions.First().EntityName);
            Assert.Equal(Account.Fields.AccountId, query2.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.ChildOf, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal(guid, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values.Single());
        }

        [Fact]
        public void ShouldSetWhereContains()
        {
            var query = new Query<Account>()
                .AddFilters(new Filter().WhereContains(Account.Fields.Name, "test"));

            Assert.Equal(Account.Fields.Name, query.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.Contains, query.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal("test", query.QueryExpression.Criteria.Filters.First().Conditions.First().Values.Single());

            var query2 = new Query<Account>()
                .AddFilters(new Filter().WhereContains(Account.Fields.Name, "test", Account.EntityLogicalName));

            Assert.Equal(Account.EntityLogicalName, query2.QueryExpression.Criteria.Filters.First().Conditions.First().EntityName);
            Assert.Equal(Account.Fields.Name, query2.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.Contains, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal("test", query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values.Single());
        }

        [Fact]
        public void ShouldSetWhereContainValues()
        {
            var query = new Query<Account>()
                .AddFilters(new Filter().WhereContainValues(Account.Fields.CustomerTypeCode, 1, 2, 3));

            Assert.Equal(Account.Fields.CustomerTypeCode, query.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.ContainValues, query.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Contains(1, query.QueryExpression.Criteria.Filters.First().Conditions.First().Values);
            Assert.Contains(2, query.QueryExpression.Criteria.Filters.First().Conditions.First().Values);
            Assert.Contains(3, query.QueryExpression.Criteria.Filters.First().Conditions.First().Values);

            var query2 = new Query<Account>()
                .AddFilters(new Filter().WhereContainValues(Account.EntityLogicalName, Account.Fields.CustomerTypeCode, 1, 2, 3));

            Assert.Equal(Account.EntityLogicalName, query2.QueryExpression.Criteria.Filters.First().Conditions.First().EntityName);
            Assert.Equal(Account.Fields.CustomerTypeCode, query2.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.ContainValues, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Contains(1, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values);
            Assert.Contains(2, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values);
            Assert.Contains(3, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereDoesNotBeginWith()
        {
            var query = new Query<Account>()
                .AddFilters(new Filter().WhereDoesNotBeginWith(Account.Fields.Name, "test"));

            Assert.Equal(Account.Fields.Name, query.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.DoesNotBeginWith, query.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal("test", query.QueryExpression.Criteria.Filters.First().Conditions.First().Values.Single());

            var query2 = new Query<Account>()
                .AddFilters(new Filter().WhereDoesNotBeginWith(Account.Fields.Name, "test", Account.EntityLogicalName));

            Assert.Equal(Account.EntityLogicalName, query2.QueryExpression.Criteria.Filters.First().Conditions.First().EntityName);
            Assert.Equal(Account.Fields.Name, query2.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.DoesNotBeginWith, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal("test", query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values.Single());
        }

        [Fact]
        public void ShouldSetWhereDoesNotContain()
        {
            var query = new Query<Account>()
                .AddFilters(new Filter().WhereDoesNotContain(Account.Fields.Name, "test"));

            Assert.Equal(Account.Fields.Name, query.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.DoesNotContain, query.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal("test", query.QueryExpression.Criteria.Filters.First().Conditions.First().Values.Single());

            var query2 = new Query<Account>()
                .AddFilters(new Filter().WhereDoesNotContain(Account.Fields.Name, "test", Account.EntityLogicalName));

            Assert.Equal(Account.EntityLogicalName, query2.QueryExpression.Criteria.Filters.First().Conditions.First().EntityName);
            Assert.Equal(Account.Fields.Name, query2.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.DoesNotContain, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal("test", query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values.Single());
        }

        [Fact]
        public void ShouldSetWhereDoesNotContainValues()
        {
            var query = new Query<Account>()
                .AddFilters(new Filter().WhereDoesNotContainValues(Account.Fields.CustomerTypeCode, 1, 2, 3));

            Assert.Equal(Account.Fields.CustomerTypeCode, query.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.DoesNotContainValues, query.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Contains(1, query.QueryExpression.Criteria.Filters.First().Conditions.First().Values);
            Assert.Contains(2, query.QueryExpression.Criteria.Filters.First().Conditions.First().Values);
            Assert.Contains(3, query.QueryExpression.Criteria.Filters.First().Conditions.First().Values);

            var query2 = new Query<Account>()
                .AddFilters(new Filter().WhereDoesNotContainValues(Account.EntityLogicalName, Account.Fields.CustomerTypeCode, 1, 2, 3));

            Assert.Equal(Account.EntityLogicalName, query2.QueryExpression.Criteria.Filters.First().Conditions.First().EntityName);
            Assert.Equal(Account.Fields.CustomerTypeCode, query2.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.DoesNotContainValues, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Contains(1, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values);
            Assert.Contains(2, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values);
            Assert.Contains(3, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereDoesNotEndWith()
        {
            var query = new Query<Account>()
                .AddFilters(new Filter().WhereDoesNotEndWith(Account.Fields.Name, "test"));

            Assert.Equal(Account.Fields.Name, query.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.DoesNotEndWith, query.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal("test", query.QueryExpression.Criteria.Filters.First().Conditions.First().Values.Single());

            var query2 = new Query<Account>()
                .AddFilters(new Filter().WhereDoesNotEndWith(Account.Fields.Name, "test", Account.EntityLogicalName));

            Assert.Equal(Account.EntityLogicalName, query2.QueryExpression.Criteria.Filters.First().Conditions.First().EntityName);
            Assert.Equal(Account.Fields.Name, query2.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.DoesNotEndWith, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal("test", query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values.Single());
        }

        [Fact]
        public void ShouldSetWhereEndsWith()
        {
            var query = new Query<Account>()
                .AddFilters(new Filter().WhereEndsWith(Account.Fields.Name, "test"));

            Assert.Equal(Account.Fields.Name, query.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.EndsWith, query.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal("test", query.QueryExpression.Criteria.Filters.First().Conditions.First().Values.Single());

            var query2 = new Query<Account>()
                .AddFilters(new Filter().WhereEndsWith(Account.Fields.Name, "test", Account.EntityLogicalName));

            Assert.Equal(Account.EntityLogicalName, query2.QueryExpression.Criteria.Filters.First().Conditions.First().EntityName);
            Assert.Equal(Account.Fields.Name, query2.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.EndsWith, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal("test", query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values.Single());
        }

        [Fact]
        public void ShouldSetWhereEqual()
        {
            var query = new Query<Account>()
                .AddFilters(new Filter().WhereEqual(Account.Fields.Name, "test"));

            Assert.Equal(Account.Fields.Name, query.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.Equal, query.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal("test", query.QueryExpression.Criteria.Filters.First().Conditions.First().Values.Single());

            var query2 = new Query<Account>()
                .AddFilters(new Filter().WhereEqual(Account.Fields.Name, "test", Account.EntityLogicalName));

            Assert.Equal(Account.EntityLogicalName, query2.QueryExpression.Criteria.Filters.First().Conditions.First().EntityName);
            Assert.Equal(Account.Fields.Name, query2.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.Equal, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal("test", query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values.Single());
        }

        [Fact]
        public void ShouldSetWhereEqualBusinessId()
        {
            var query = new Query<Account>()
                .AddFilters(new Filter().WhereEqualBusinessId(Account.Fields.OwningBusinessUnit));

            Assert.Equal(Account.Fields.OwningBusinessUnit, query.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.EqualBusinessId, query.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Empty(query.QueryExpression.Criteria.Filters.First().Conditions.First().Values);

            var query2 = new Query<Account>()
                .AddFilters(new Filter().WhereEqualBusinessId(Account.Fields.OwningBusinessUnit, Account.EntityLogicalName));

            Assert.Equal(Account.EntityLogicalName, query2.QueryExpression.Criteria.Filters.First().Conditions.First().EntityName);
            Assert.Equal(Account.Fields.OwningBusinessUnit, query2.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.EqualBusinessId, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Empty(query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereEqualUserId()
        {
            var query = new Query<Account>()
                .AddFilters(new Filter().WhereEqualUserId(Account.Fields.OwnerId));

            Assert.Equal(Account.Fields.OwnerId, query.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.EqualUserId, query.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Empty(query.QueryExpression.Criteria.Filters.First().Conditions.First().Values);

            var query2 = new Query<Account>()
                .AddFilters(new Filter().WhereEqualUserId(Account.Fields.OwnerId, Account.EntityLogicalName));

            Assert.Equal(Account.EntityLogicalName, query2.QueryExpression.Criteria.Filters.First().Conditions.First().EntityName);
            Assert.Equal(Account.Fields.OwnerId, query2.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.EqualUserId, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Empty(query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereEqualUserLanguage()
        {
            var query = new Query<Account>()
                .AddFilters(new Filter().WhereEqualUserLanguage("no_language_attribute"));

            Assert.Equal("no_language_attribute", query.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.EqualUserLanguage, query.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Empty(query.QueryExpression.Criteria.Filters.First().Conditions.First().Values);

            var query2 = new Query<Account>()
                .AddFilters(new Filter().WhereEqualUserLanguage("no_language_attribute", Account.EntityLogicalName));

            Assert.Equal(Account.EntityLogicalName, query2.QueryExpression.Criteria.Filters.First().Conditions.First().EntityName);
            Assert.Equal("no_language_attribute", query2.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.EqualUserLanguage, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Empty(query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereEqualUserOrUserHierarchy()
        {
            var query = new Query<Account>()
                .AddFilters(new Filter().WhereEqualUserOrUserHierarchy(Account.Fields.OwnerId));

            Assert.Equal(Account.Fields.OwnerId, query.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.EqualUserOrUserHierarchy, query.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Empty(query.QueryExpression.Criteria.Filters.First().Conditions.First().Values);

            var query2 = new Query<Account>()
                .AddFilters(new Filter().WhereEqualUserOrUserHierarchy(Account.Fields.OwnerId, Account.EntityLogicalName));

            Assert.Equal(Account.EntityLogicalName, query2.QueryExpression.Criteria.Filters.First().Conditions.First().EntityName);
            Assert.Equal(Account.Fields.OwnerId, query2.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.EqualUserOrUserHierarchy, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Empty(query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereEqualUserOrUserHierarchyAndTeams()
        {
            var query = new Query<Account>()
                .AddFilters(new Filter().WhereEqualUserOrUserHierarchyAndTeams(Account.Fields.OwnerId));

            Assert.Equal(Account.Fields.OwnerId, query.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.EqualUserOrUserHierarchyAndTeams, query.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Empty(query.QueryExpression.Criteria.Filters.First().Conditions.First().Values);

            var query2 = new Query<Account>()
                .AddFilters(new Filter().WhereEqualUserOrUserHierarchyAndTeams(Account.Fields.OwnerId, Account.EntityLogicalName));

            Assert.Equal(Account.EntityLogicalName, query2.QueryExpression.Criteria.Filters.First().Conditions.First().EntityName);
            Assert.Equal(Account.Fields.OwnerId, query2.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.EqualUserOrUserHierarchyAndTeams, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Empty(query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereEqualUserOrUserTeams()
        {
            var query = new Query<Account>()
                .AddFilters(new Filter().WhereEqualUserOrUserTeams(Account.Fields.OwnerId));

            Assert.Equal(Account.Fields.OwnerId, query.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.EqualUserOrUserTeams, query.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Empty(query.QueryExpression.Criteria.Filters.First().Conditions.First().Values);

            var query2 = new Query<Account>()
                .AddFilters(new Filter().WhereEqualUserOrUserTeams(Account.Fields.OwnerId, Account.EntityLogicalName));

            Assert.Equal(Account.EntityLogicalName, query2.QueryExpression.Criteria.Filters.First().Conditions.First().EntityName);
            Assert.Equal(Account.Fields.OwnerId, query2.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.EqualUserOrUserTeams, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Empty(query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereEqualUserTeams()
        {
            var query = new Query<Account>()
                .AddFilters(new Filter().WhereEqualUserTeams(Account.Fields.OwnerId));

            Assert.Equal(Account.Fields.OwnerId, query.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.EqualUserTeams, query.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Empty(query.QueryExpression.Criteria.Filters.First().Conditions.First().Values);

            var query2 = new Query<Account>()
                .AddFilters(new Filter().WhereEqualUserTeams(Account.Fields.OwnerId, Account.EntityLogicalName));

            Assert.Equal(Account.EntityLogicalName, query2.QueryExpression.Criteria.Filters.First().Conditions.First().EntityName);
            Assert.Equal(Account.Fields.OwnerId, query2.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.EqualUserTeams, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Empty(query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereGreaterEqual()
        {
            var query = new Query<Account>()
                .AddFilters(new Filter().WhereGreaterEqual(Account.Fields.NumberOfEmployees, 10));

            Assert.Equal(Account.Fields.NumberOfEmployees, query.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.GreaterEqual, query.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal(10, query.QueryExpression.Criteria.Filters.First().Conditions.First().Values.First());

            var query2 = new Query<Account>()
                .AddFilters(new Filter().WhereGreaterEqual(Account.Fields.NumberOfEmployees, 10, Account.EntityLogicalName));

            Assert.Equal(Account.EntityLogicalName, query2.QueryExpression.Criteria.Filters.First().Conditions.First().EntityName);
            Assert.Equal(Account.Fields.NumberOfEmployees, query2.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.GreaterEqual, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal(10, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values.First());
        }

        [Fact]
        public void ShouldSetWhereGreaterThan()
        {
            var query = new Query<Account>()
                .AddFilters(new Filter().WhereGreaterThan(Account.Fields.NumberOfEmployees, 10));

            Assert.Equal(Account.Fields.NumberOfEmployees, query.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.GreaterThan, query.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal(10, query.QueryExpression.Criteria.Filters.First().Conditions.First().Values.First());

            var query2 = new Query<Account>()
                .AddFilters(new Filter().WhereGreaterThan(Account.Fields.NumberOfEmployees, 10, Account.EntityLogicalName));

            Assert.Equal(Account.EntityLogicalName, query2.QueryExpression.Criteria.Filters.First().Conditions.First().EntityName);
            Assert.Equal(Account.Fields.NumberOfEmployees, query2.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.GreaterThan, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal(10, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values.First());
        }

        [Fact]
        public void ShouldSetWhereIn()
        {
            var query = new Query<Account>()
                .AddFilters(new Filter().WhereIn(Account.Fields.CustomerTypeCode, 1, 2, 3));

            Assert.Equal(Account.Fields.CustomerTypeCode, query.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.In, query.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Contains(1, query.QueryExpression.Criteria.Filters.First().Conditions.First().Values);
            Assert.Contains(2, query.QueryExpression.Criteria.Filters.First().Conditions.First().Values);
            Assert.Contains(3, query.QueryExpression.Criteria.Filters.First().Conditions.First().Values);

            var query2 = new Query<Account>()
                .AddFilters(new Filter().WhereIn(Account.EntityLogicalName, Account.Fields.CustomerTypeCode, 1, 2, 3));

            Assert.Equal(Account.EntityLogicalName, query2.QueryExpression.Criteria.Filters.First().Conditions.First().EntityName);
            Assert.Equal(Account.Fields.CustomerTypeCode, query2.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.In, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Contains(1, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values);
            Assert.Contains(2, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values);
            Assert.Contains(3, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereInFiscalPeriod()
        {
            var query = new Query<Account>()
                .AddFilters(new Filter().WhereInFiscalPeriod(Account.Fields.CreatedOn, 1));

            Assert.Equal(Account.Fields.CreatedOn, query.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.InFiscalPeriod, query.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal(1, query.QueryExpression.Criteria.Filters.First().Conditions.First().Values.First());

            var query2 = new Query<Account>()
                .AddFilters(new Filter().WhereInFiscalPeriod(Account.Fields.CreatedOn, 1, Account.EntityLogicalName));

            Assert.Equal(Account.EntityLogicalName, query2.QueryExpression.Criteria.Filters.First().Conditions.First().EntityName);
            Assert.Equal(Account.Fields.CreatedOn, query2.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.InFiscalPeriod, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal(1, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values.First());
        }

        [Fact]
        public void ShouldSetWhereInFiscalPeriodAndYear()
        {
            var query = new Query<Account>()
                .AddFilters(new Filter().WhereInFiscalPeriodAndYear(Account.Fields.CreatedOn, 1, 2018));

            Assert.Equal(Account.Fields.CreatedOn, query.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.InFiscalPeriodAndYear, query.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal(1, query.QueryExpression.Criteria.Filters.First().Conditions.First().Values[0]);
            Assert.Equal(2018, query.QueryExpression.Criteria.Filters.First().Conditions.First().Values[1]);

            var query2 = new Query<Account>()
                .AddFilters(new Filter().WhereInFiscalPeriodAndYear(Account.Fields.CreatedOn, 1, 2018, Account.EntityLogicalName));

            Assert.Equal(Account.EntityLogicalName, query2.QueryExpression.Criteria.Filters.First().Conditions.First().EntityName);
            Assert.Equal(Account.Fields.CreatedOn, query2.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.InFiscalPeriodAndYear, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal(1, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values[0]);
            Assert.Equal(2018, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values[1]);
        }

        [Fact]
        public void ShouldSetWhereInFiscalYear()
        {
            var query = new Query<Account>()
                .AddFilters(new Filter().WhereInFiscalYear(Account.Fields.CreatedOn, 2018));

            Assert.Equal(Account.Fields.CreatedOn, query.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.InFiscalYear, query.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal(2018, query.QueryExpression.Criteria.Filters.First().Conditions.First().Values.First());

            var query2 = new Query<Account>()
                .AddFilters(new Filter().WhereInFiscalYear(Account.Fields.CreatedOn, 2018, Account.EntityLogicalName));

            Assert.Equal(Account.EntityLogicalName, query2.QueryExpression.Criteria.Filters.First().Conditions.First().EntityName);
            Assert.Equal(Account.Fields.CreatedOn, query2.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.InFiscalYear, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal(2018, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values.First());
        }

        [Fact]
        public void ShouldSetWhereInList()
        {
            var query = new Query<Account>()
                .AddFilters(new Filter().WhereIn(Account.Fields.CustomerTypeCode, new List<int> { 1, 2, 3 }));

            Assert.Equal(Account.Fields.CustomerTypeCode, query.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.In, query.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.IsAssignableFrom<IList>(query.QueryExpression.Criteria.Filters.First().Conditions.First().Values);

            var query2 = new Query<Account>()
                .AddFilters(new Filter().WhereIn(Account.EntityLogicalName, Account.Fields.CustomerTypeCode, new List<int> { 1, 2, 3 }));

            Assert.Equal(Account.EntityLogicalName, query2.QueryExpression.Criteria.Filters.First().Conditions.First().EntityName);
            Assert.Equal(Account.Fields.CustomerTypeCode, query2.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.In, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.IsAssignableFrom<IList>(query.QueryExpression.Criteria.Filters.First().Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereInOrAfterFiscalPeriodAndYear()
        {
            var query = new Query<Account>()
                .AddFilters(new Filter().WhereInOrAfterFiscalPeriodAndYear(Account.Fields.CreatedOn, 1, 2018));

            Assert.Equal(Account.Fields.CreatedOn, query.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.InOrAfterFiscalPeriodAndYear, query.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal(1, query.QueryExpression.Criteria.Filters.First().Conditions.First().Values[0]);
            Assert.Equal(2018, query.QueryExpression.Criteria.Filters.First().Conditions.First().Values[1]);

            var query2 = new Query<Account>()
                .AddFilters(new Filter().WhereInOrAfterFiscalPeriodAndYear(Account.Fields.CreatedOn, 1, 2018, Account.EntityLogicalName));

            Assert.Equal(Account.EntityLogicalName, query2.QueryExpression.Criteria.Filters.First().Conditions.First().EntityName);
            Assert.Equal(Account.Fields.CreatedOn, query2.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.InOrAfterFiscalPeriodAndYear, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal(1, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values[0]);
            Assert.Equal(2018, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values[1]);
        }

        [Fact]
        public void ShouldSetWhereInOrBeforeFiscalPeriodAndYear()
        {
            var query = new Query<Account>()
                .AddFilters(new Filter().WhereInOrBeforeFiscalPeriodAndYear(Account.Fields.CreatedOn, 1, 2018));

            Assert.Equal(Account.Fields.CreatedOn, query.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.InOrBeforeFiscalPeriodAndYear, query.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal(1, query.QueryExpression.Criteria.Filters.First().Conditions.First().Values[0]);
            Assert.Equal(2018, query.QueryExpression.Criteria.Filters.First().Conditions.First().Values[1]);

            var query2 = new Query<Account>()
                .AddFilters(new Filter().WhereInOrBeforeFiscalPeriodAndYear(Account.Fields.CreatedOn, 1, 2018, Account.EntityLogicalName));

            Assert.Equal(Account.EntityLogicalName, query2.QueryExpression.Criteria.Filters.First().Conditions.First().EntityName);
            Assert.Equal(Account.Fields.CreatedOn, query2.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.InOrBeforeFiscalPeriodAndYear, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal(1, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values[0]);
            Assert.Equal(2018, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values[1]);
        }

        [Fact]
        public void ShouldSetWhereLast7Days()
        {
            var query = new Query<Account>()
                .AddFilters(new Filter().WhereLast7Days(Account.Fields.CreatedOn));

            Assert.Equal(Account.Fields.CreatedOn, query.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.Last7Days, query.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Empty(query.QueryExpression.Criteria.Filters.First().Conditions.First().Values);

            var query2 = new Query<Account>()
                .AddFilters(new Filter().WhereLast7Days(Account.Fields.CreatedOn, Account.EntityLogicalName));

            Assert.Equal(Account.EntityLogicalName, query2.QueryExpression.Criteria.Filters.First().Conditions.First().EntityName);
            Assert.Equal(Account.Fields.CreatedOn, query2.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.Last7Days, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Empty(query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereLastFiscalPeriod()
        {
            var query = new Query<Account>()
                .AddFilters(new Filter().WhereLastFiscalPeriod(Account.Fields.CreatedOn));

            Assert.Equal(Account.Fields.CreatedOn, query.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.LastFiscalPeriod, query.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Empty(query.QueryExpression.Criteria.Filters.First().Conditions.First().Values);

            var query2 = new Query<Account>()
                .AddFilters(new Filter().WhereLastFiscalPeriod(Account.Fields.CreatedOn, Account.EntityLogicalName));

            Assert.Equal(Account.EntityLogicalName, query2.QueryExpression.Criteria.Filters.First().Conditions.First().EntityName);
            Assert.Equal(Account.Fields.CreatedOn, query2.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.LastFiscalPeriod, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Empty(query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereLastFiscalYear()
        {
            var query = new Query<Account>()
                .AddFilters(new Filter().WhereLastFiscalYear(Account.Fields.CreatedOn));

            Assert.Equal(Account.Fields.CreatedOn, query.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.LastFiscalYear, query.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Empty(query.QueryExpression.Criteria.Filters.First().Conditions.First().Values);

            var query2 = new Query<Account>()
                .AddFilters(new Filter().WhereLastFiscalYear(Account.Fields.CreatedOn, Account.EntityLogicalName));

            Assert.Equal(Account.EntityLogicalName, query2.QueryExpression.Criteria.Filters.First().Conditions.First().EntityName);
            Assert.Equal(Account.Fields.CreatedOn, query2.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.LastFiscalYear, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Empty(query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereLastMonth()
        {
            var query = new Query<Account>()
                .AddFilters(new Filter().WhereLastMonth(Account.Fields.CreatedOn));

            Assert.Equal(Account.Fields.CreatedOn, query.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.LastMonth, query.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Empty(query.QueryExpression.Criteria.Filters.First().Conditions.First().Values);

            var query2 = new Query<Account>()
                .AddFilters(new Filter().WhereLastMonth(Account.Fields.CreatedOn, Account.EntityLogicalName));

            Assert.Equal(Account.EntityLogicalName, query2.QueryExpression.Criteria.Filters.First().Conditions.First().EntityName);
            Assert.Equal(Account.Fields.CreatedOn, query2.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.LastMonth, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Empty(query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereLastWeek()
        {
            var query = new Query<Account>()
                .AddFilters(new Filter().WhereLastWeek(Account.Fields.CreatedOn));

            Assert.Equal(Account.Fields.CreatedOn, query.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.LastWeek, query.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Empty(query.QueryExpression.Criteria.Filters.First().Conditions.First().Values);

            var query2 = new Query<Account>()
                .AddFilters(new Filter().WhereLastWeek(Account.Fields.CreatedOn, Account.EntityLogicalName));

            Assert.Equal(Account.EntityLogicalName, query2.QueryExpression.Criteria.Filters.First().Conditions.First().EntityName);
            Assert.Equal(Account.Fields.CreatedOn, query2.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.LastWeek, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Empty(query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereLastXDays()
        {
            var query = new Query<Account>()
                .AddFilters(new Filter().WhereLastXDays(Account.Fields.CreatedOn, 10));

            Assert.Equal(Account.Fields.CreatedOn, query.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.LastXDays, query.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal(10, query.QueryExpression.Criteria.Filters.First().Conditions.First().Values.First());

            var query2 = new Query<Account>()
                .AddFilters(new Filter().WhereLastXDays(Account.Fields.CreatedOn, 10, Account.EntityLogicalName));

            Assert.Equal(Account.EntityLogicalName, query2.QueryExpression.Criteria.Filters.First().Conditions.First().EntityName);
            Assert.Equal(Account.Fields.CreatedOn, query2.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.LastXDays, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal(10, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values.First());
        }

        [Fact]
        public void ShouldSetWhereLastXFiscalPeriods()
        {
            var query = new Query<Account>()
                .AddFilters(new Filter().WhereLastXFiscalPeriods(Account.Fields.CreatedOn, 10));

            Assert.Equal(Account.Fields.CreatedOn, query.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.LastXFiscalPeriods, query.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal(10, query.QueryExpression.Criteria.Filters.First().Conditions.First().Values.First());

            var query2 = new Query<Account>()
                .AddFilters(new Filter().WhereLastXFiscalPeriods(Account.Fields.CreatedOn, 10, Account.EntityLogicalName));

            Assert.Equal(Account.EntityLogicalName, query2.QueryExpression.Criteria.Filters.First().Conditions.First().EntityName);
            Assert.Equal(Account.Fields.CreatedOn, query2.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.LastXFiscalPeriods, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal(10, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values.First());
        }

        [Fact]
        public void ShouldSetWhereLastXFiscalYears()
        {
            var query = new Query<Account>()
                .AddFilters(new Filter().WhereLastXFiscalYears(Account.Fields.CreatedOn, 10));

            Assert.Equal(Account.Fields.CreatedOn, query.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.LastXFiscalYears, query.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal(10, query.QueryExpression.Criteria.Filters.First().Conditions.First().Values.First());

            var query2 = new Query<Account>()
                .AddFilters(new Filter().WhereLastXFiscalYears(Account.Fields.CreatedOn, 10, Account.EntityLogicalName));

            Assert.Equal(Account.EntityLogicalName, query2.QueryExpression.Criteria.Filters.First().Conditions.First().EntityName);
            Assert.Equal(Account.Fields.CreatedOn, query2.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.LastXFiscalYears, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal(10, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values.First());
        }

        [Fact]
        public void ShouldSetWhereLastXHours()
        {
            var query = new Query<Account>()
                .AddFilters(new Filter().WhereLastXHours(Account.Fields.CreatedOn, 10));

            Assert.Equal(Account.Fields.CreatedOn, query.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.LastXHours, query.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal(10, query.QueryExpression.Criteria.Filters.First().Conditions.First().Values.First());

            var query2 = new Query<Account>()
                .AddFilters(new Filter().WhereLastXHours(Account.Fields.CreatedOn, 10, Account.EntityLogicalName));

            Assert.Equal(Account.EntityLogicalName, query2.QueryExpression.Criteria.Filters.First().Conditions.First().EntityName);
            Assert.Equal(Account.Fields.CreatedOn, query2.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.LastXHours, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal(10, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values.First());
        }

        [Fact]
        public void ShouldSetWhereLastXMonths()
        {
            var query = new Query<Account>()
                .AddFilters(new Filter().WhereLastXMonths(Account.Fields.CreatedOn, 10));

            Assert.Equal(Account.Fields.CreatedOn, query.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.LastXMonths, query.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal(10, query.QueryExpression.Criteria.Filters.First().Conditions.First().Values.First());

            var query2 = new Query<Account>()
                .AddFilters(new Filter().WhereLastXMonths(Account.Fields.CreatedOn, 10, Account.EntityLogicalName));

            Assert.Equal(Account.EntityLogicalName, query2.QueryExpression.Criteria.Filters.First().Conditions.First().EntityName);
            Assert.Equal(Account.Fields.CreatedOn, query2.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.LastXMonths, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal(10, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values.First());
        }

        [Fact]
        public void ShouldSetWhereLastXWeeks()
        {
            var query = new Query<Account>()
                .AddFilters(new Filter().WhereLastXWeeks(Account.Fields.CreatedOn, 10));

            Assert.Equal(Account.Fields.CreatedOn, query.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.LastXWeeks, query.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal(10, query.QueryExpression.Criteria.Filters.First().Conditions.First().Values.First());

            var query2 = new Query<Account>()
                .AddFilters(new Filter().WhereLastXWeeks(Account.Fields.CreatedOn, 10, Account.EntityLogicalName));

            Assert.Equal(Account.EntityLogicalName, query2.QueryExpression.Criteria.Filters.First().Conditions.First().EntityName);
            Assert.Equal(Account.Fields.CreatedOn, query2.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.LastXWeeks, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal(10, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values.First());
        }

        [Fact]
        public void ShouldSetWhereLastXYears()
        {
            var query = new Query<Account>()
                .AddFilters(new Filter().WhereLastXYears(Account.Fields.CreatedOn, 10));

            Assert.Equal(Account.Fields.CreatedOn, query.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.LastXYears, query.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal(10, query.QueryExpression.Criteria.Filters.First().Conditions.First().Values.First());

            var query2 = new Query<Account>()
                .AddFilters(new Filter().WhereLastXYears(Account.Fields.CreatedOn, 10, Account.EntityLogicalName));

            Assert.Equal(Account.EntityLogicalName, query2.QueryExpression.Criteria.Filters.First().Conditions.First().EntityName);
            Assert.Equal(Account.Fields.CreatedOn, query2.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.LastXYears, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal(10, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values.First());
        }

        [Fact]
        public void ShouldSetWhereLastYear()
        {
            var query = new Query<Account>()
                .AddFilters(new Filter().WhereLastYear(Account.Fields.CreatedOn));

            Assert.Equal(Account.Fields.CreatedOn, query.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.LastYear, query.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Empty(query.QueryExpression.Criteria.Filters.First().Conditions.First().Values);

            var query2 = new Query<Account>()
                .AddFilters(new Filter().WhereLastYear(Account.Fields.CreatedOn, Account.EntityLogicalName));

            Assert.Equal(Account.EntityLogicalName, query2.QueryExpression.Criteria.Filters.First().Conditions.First().EntityName);
            Assert.Equal(Account.Fields.CreatedOn, query2.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.LastYear, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Empty(query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereLessEqual()
        {
            var query = new Query<Account>()
                .AddFilters(new Filter().WhereLessEqual(Account.Fields.NumberOfEmployees, 10));

            Assert.Equal(Account.Fields.NumberOfEmployees, query.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.LessEqual, query.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal(10, query.QueryExpression.Criteria.Filters.First().Conditions.First().Values.First());

            var query2 = new Query<Account>()
                .AddFilters(new Filter().WhereLessEqual(Account.Fields.NumberOfEmployees, 10, Account.EntityLogicalName));

            Assert.Equal(Account.EntityLogicalName, query2.QueryExpression.Criteria.Filters.First().Conditions.First().EntityName);
            Assert.Equal(Account.Fields.NumberOfEmployees, query2.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.LessEqual, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal(10, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values.First());
        }

        [Fact]
        public void ShouldSetWhereLessThan()
        {
            var query = new Query<Account>()
                .AddFilters(new Filter().WhereLessThan(Account.Fields.NumberOfEmployees, 10));

            Assert.Equal(Account.Fields.NumberOfEmployees, query.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.LessThan, query.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal(10, query.QueryExpression.Criteria.Filters.First().Conditions.First().Values.First());

            var query2 = new Query<Account>()
                .AddFilters(new Filter().WhereLessThan(Account.Fields.NumberOfEmployees, 10, Account.EntityLogicalName));

            Assert.Equal(Account.EntityLogicalName, query2.QueryExpression.Criteria.Filters.First().Conditions.First().EntityName);
            Assert.Equal(Account.Fields.NumberOfEmployees, query2.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.LessThan, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal(10, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values.First());
        }

        [Fact]
        public void ShouldSetWhereLike()
        {
            var query = new Query<Account>()
                .AddFilters(new Filter().WhereLike(Account.Fields.Name, "%test%"));

            Assert.Equal(Account.Fields.Name, query.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.Like, query.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal("%test%", query.QueryExpression.Criteria.Filters.First().Conditions.First().Values.First());

            var query2 = new Query<Account>()
                .AddFilters(new Filter().WhereLike(Account.Fields.Name, "%test%", Account.EntityLogicalName));

            Assert.Equal(Account.EntityLogicalName, query2.QueryExpression.Criteria.Filters.First().Conditions.First().EntityName);
            Assert.Equal(Account.Fields.Name, query2.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.Like, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal("%test%", query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values.First());
        }

        [Fact]
        public void ShouldSetWhereMask()
        {
            var obj = new object();
            var query = new Query<Account>()
                .AddFilters(new Filter().WhereMask(Account.Fields.Name, obj));

            Assert.Equal(Account.Fields.Name, query.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.Mask, query.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal(query.QueryExpression.Criteria.Filters.First().Conditions.First().Values.First(), obj);

            var query2 = new Query<Account>()
                .AddFilters(new Filter().WhereMask(Account.Fields.Name, obj, Account.EntityLogicalName));

            Assert.Equal(Account.EntityLogicalName, query2.QueryExpression.Criteria.Filters.First().Conditions.First().EntityName);
            Assert.Equal(Account.Fields.Name, query2.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.Mask, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal(query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values.First(), obj);
        }

        [Fact]
        public void ShouldSetWhereMasksSelect()
        {
            var obj = new object();
            var query = new Query<Account>()
                .AddFilters(new Filter().WhereMasksSelect(Account.Fields.Name, obj));

            Assert.Equal(Account.Fields.Name, query.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.MasksSelect, query.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal(query.QueryExpression.Criteria.Filters.First().Conditions.First().Values.First(), obj);

            var query2 = new Query<Account>()
                .AddFilters(new Filter().WhereMasksSelect(Account.Fields.Name, obj, Account.EntityLogicalName));

            Assert.Equal(Account.EntityLogicalName, query2.QueryExpression.Criteria.Filters.First().Conditions.First().EntityName);
            Assert.Equal(Account.Fields.Name, query2.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.MasksSelect, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal(query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values.First(), obj);
        }

        [Fact]
        public void ShouldSetWhereNext7Days()
        {
            var query = new Query<Account>()
                .AddFilters(new Filter().WhereNext7Days(Account.Fields.CreatedOn));

            Assert.Equal(Account.Fields.CreatedOn, query.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.Next7Days, query.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Empty(query.QueryExpression.Criteria.Filters.First().Conditions.First().Values);

            var query2 = new Query<Account>()
                .AddFilters(new Filter().WhereNext7Days(Account.Fields.CreatedOn, Account.EntityLogicalName));

            Assert.Equal(Account.EntityLogicalName, query2.QueryExpression.Criteria.Filters.First().Conditions.First().EntityName);
            Assert.Equal(Account.Fields.CreatedOn, query2.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.Next7Days, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Empty(query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereNextFiscalPeriod()
        {
            var query = new Query<Account>()
                .AddFilters(new Filter().WhereNextFiscalPeriod(Account.Fields.CreatedOn));

            Assert.Equal(Account.Fields.CreatedOn, query.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.NextFiscalPeriod, query.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Empty(query.QueryExpression.Criteria.Filters.First().Conditions.First().Values);

            var query2 = new Query<Account>()
                .AddFilters(new Filter().WhereNextFiscalPeriod(Account.Fields.CreatedOn, Account.EntityLogicalName));

            Assert.Equal(Account.EntityLogicalName, query2.QueryExpression.Criteria.Filters.First().Conditions.First().EntityName);
            Assert.Equal(Account.Fields.CreatedOn, query2.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.NextFiscalPeriod, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Empty(query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereNextFiscalYear()
        {
            var query = new Query<Account>()
                .AddFilters(new Filter().WhereNextFiscalYear(Account.Fields.CreatedOn));

            Assert.Equal(Account.Fields.CreatedOn, query.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.NextFiscalYear, query.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Empty(query.QueryExpression.Criteria.Filters.First().Conditions.First().Values);

            var query2 = new Query<Account>()
                .AddFilters(new Filter().WhereNextFiscalYear(Account.Fields.CreatedOn, Account.EntityLogicalName));

            Assert.Equal(Account.EntityLogicalName, query2.QueryExpression.Criteria.Filters.First().Conditions.First().EntityName);
            Assert.Equal(Account.Fields.CreatedOn, query2.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.NextFiscalYear, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Empty(query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereNextMonth()
        {
            var query = new Query<Account>()
                .AddFilters(new Filter().WhereNextMonth(Account.Fields.CreatedOn));

            Assert.Equal(Account.Fields.CreatedOn, query.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.NextMonth, query.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Empty(query.QueryExpression.Criteria.Filters.First().Conditions.First().Values);

            var query2 = new Query<Account>()
                .AddFilters(new Filter().WhereNextMonth(Account.Fields.CreatedOn, Account.EntityLogicalName));

            Assert.Equal(Account.EntityLogicalName, query2.QueryExpression.Criteria.Filters.First().Conditions.First().EntityName);
            Assert.Equal(Account.Fields.CreatedOn, query2.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.NextMonth, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Empty(query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereNextWeek()
        {
            var query = new Query<Account>()
                .AddFilters(new Filter().WhereNextWeek(Account.Fields.CreatedOn));

            Assert.Equal(Account.Fields.CreatedOn, query.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.NextWeek, query.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Empty(query.QueryExpression.Criteria.Filters.First().Conditions.First().Values);

            var query2 = new Query<Account>()
                .AddFilters(new Filter().WhereNextWeek(Account.Fields.CreatedOn, Account.EntityLogicalName));

            Assert.Equal(Account.EntityLogicalName, query2.QueryExpression.Criteria.Filters.First().Conditions.First().EntityName);
            Assert.Equal(Account.Fields.CreatedOn, query2.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.NextWeek, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Empty(query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereNextXDays()
        {
            var query = new Query<Account>()
                .AddFilters(new Filter().WhereNextXDays(Account.Fields.CreatedOn, 10));

            Assert.Equal(Account.Fields.CreatedOn, query.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.NextXDays, query.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal(10, query.QueryExpression.Criteria.Filters.First().Conditions.First().Values.First());

            var query2 = new Query<Account>()
                .AddFilters(new Filter().WhereNextXDays(Account.Fields.CreatedOn, 10, Account.EntityLogicalName));

            Assert.Equal(Account.EntityLogicalName, query2.QueryExpression.Criteria.Filters.First().Conditions.First().EntityName);
            Assert.Equal(Account.Fields.CreatedOn, query2.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.NextXDays, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal(10, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values.First());
        }

        [Fact]
        public void ShouldSetWhereNextXFiscalPeriods()
        {
            var query = new Query<Account>()
                .AddFilters(new Filter().WhereNextXFiscalPeriods(Account.Fields.CreatedOn, 10));

            Assert.Equal(Account.Fields.CreatedOn, query.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.NextXFiscalPeriods, query.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal(10, query.QueryExpression.Criteria.Filters.First().Conditions.First().Values.First());

            var query2 = new Query<Account>()
                .AddFilters(new Filter().WhereNextXFiscalPeriods(Account.Fields.CreatedOn, 10, Account.EntityLogicalName));

            Assert.Equal(Account.EntityLogicalName, query2.QueryExpression.Criteria.Filters.First().Conditions.First().EntityName);
            Assert.Equal(Account.Fields.CreatedOn, query2.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.NextXFiscalPeriods, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal(10, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values.First());
        }

        [Fact]
        public void ShouldSetWhereNextXFiscalYears()
        {
            var query = new Query<Account>()
                .AddFilters(new Filter().WhereNextXFiscalYears(Account.Fields.CreatedOn, 10));

            Assert.Equal(Account.Fields.CreatedOn, query.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.NextXFiscalYears, query.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal(10, query.QueryExpression.Criteria.Filters.First().Conditions.First().Values.First());

            var query2 = new Query<Account>()
                .AddFilters(new Filter().WhereNextXFiscalYears(Account.Fields.CreatedOn, 10, Account.EntityLogicalName));

            Assert.Equal(Account.EntityLogicalName, query2.QueryExpression.Criteria.Filters.First().Conditions.First().EntityName);
            Assert.Equal(Account.Fields.CreatedOn, query2.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.NextXFiscalYears, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal(10, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values.First());
        }

        [Fact]
        public void ShouldSetWhereNextXHours()
        {
            var query = new Query<Account>()
                .AddFilters(new Filter().WhereNextXHours(Account.Fields.CreatedOn, 10));

            Assert.Equal(Account.Fields.CreatedOn, query.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.NextXHours, query.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal(10, query.QueryExpression.Criteria.Filters.First().Conditions.First().Values.First());

            var query2 = new Query<Account>()
                .AddFilters(new Filter().WhereNextXHours(Account.Fields.CreatedOn, 10, Account.EntityLogicalName));

            Assert.Equal(Account.EntityLogicalName, query2.QueryExpression.Criteria.Filters.First().Conditions.First().EntityName);
            Assert.Equal(Account.Fields.CreatedOn, query2.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.NextXHours, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal(10, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values.First());
        }

        [Fact]
        public void ShouldSetWhereNextXMonths()
        {
            var query = new Query<Account>()
                .AddFilters(new Filter().WhereNextXMonths(Account.Fields.CreatedOn, 10));

            Assert.Equal(Account.Fields.CreatedOn, query.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.NextXMonths, query.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal(10, query.QueryExpression.Criteria.Filters.First().Conditions.First().Values.First());

            var query2 = new Query<Account>()
                .AddFilters(new Filter().WhereNextXMonths(Account.Fields.CreatedOn, 10, Account.EntityLogicalName));

            Assert.Equal(Account.EntityLogicalName, query2.QueryExpression.Criteria.Filters.First().Conditions.First().EntityName);
            Assert.Equal(Account.Fields.CreatedOn, query2.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.NextXMonths, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal(10, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values.First());
        }

        [Fact]
        public void ShouldSetWhereNextXWeeks()
        {
            var query = new Query<Account>()
                .AddFilters(new Filter().WhereNextXWeeks(Account.Fields.CreatedOn, 10));

            Assert.Equal(Account.Fields.CreatedOn, query.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.NextXWeeks, query.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal(10, query.QueryExpression.Criteria.Filters.First().Conditions.First().Values.First());

            var query2 = new Query<Account>()
                .AddFilters(new Filter().WhereNextXWeeks(Account.Fields.CreatedOn, 10, Account.EntityLogicalName));

            Assert.Equal(Account.EntityLogicalName, query2.QueryExpression.Criteria.Filters.First().Conditions.First().EntityName);
            Assert.Equal(Account.Fields.CreatedOn, query2.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.NextXWeeks, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal(10, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values.First());
        }

        [Fact]
        public void ShouldSetWhereNextXYears()
        {
            var query = new Query<Account>()
                .AddFilters(new Filter().WhereNextXYears(Account.Fields.CreatedOn, 10));

            Assert.Equal(Account.Fields.CreatedOn, query.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.NextXYears, query.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal(10, query.QueryExpression.Criteria.Filters.First().Conditions.First().Values.First());

            var query2 = new Query<Account>()
                .AddFilters(new Filter().WhereNextXYears(Account.Fields.CreatedOn, 10, Account.EntityLogicalName));

            Assert.Equal(Account.EntityLogicalName, query2.QueryExpression.Criteria.Filters.First().Conditions.First().EntityName);
            Assert.Equal(Account.Fields.CreatedOn, query2.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.NextXYears, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal(10, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values.First());
        }

        [Fact]
        public void ShouldSetWhereNextYear()
        {
            var query = new Query<Account>()
                .AddFilters(new Filter().WhereNextYear(Account.Fields.CreatedOn));

            Assert.Equal(Account.Fields.CreatedOn, query.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.NextYear, query.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Empty(query.QueryExpression.Criteria.Filters.First().Conditions.First().Values);

            var query2 = new Query<Account>()
                .AddFilters(new Filter().WhereNextYear(Account.Fields.CreatedOn, Account.EntityLogicalName));

            Assert.Equal(Account.EntityLogicalName, query2.QueryExpression.Criteria.Filters.First().Conditions.First().EntityName);
            Assert.Equal(Account.Fields.CreatedOn, query2.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.NextYear, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Empty(query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereNotBetween()
        {
            var query = new Query<Account>()
                .AddFilters(new Filter().WhereNotBetween(Account.Fields.NumberOfEmployees, 10, 50));

            Assert.Equal(Account.Fields.NumberOfEmployees, query.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.NotBetween, query.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal(10, query.QueryExpression.Criteria.Filters.First().Conditions.First().Values[0]);
            Assert.Equal(50, query.QueryExpression.Criteria.Filters.First().Conditions.First().Values[1]);

            var query2 = new Query<Account>()
                .AddFilters(new Filter().WhereNotBetween(Account.Fields.NumberOfEmployees, 10, 50, Account.EntityLogicalName));

            Assert.Equal(Account.EntityLogicalName, query2.QueryExpression.Criteria.Filters.First().Conditions.First().EntityName);
            Assert.Equal(Account.Fields.NumberOfEmployees, query2.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.NotBetween, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal(10, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values[0]);
            Assert.Equal(50, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values[1]);
        }

        [Fact]
        public void ShouldSetWhereNotEqual()
        {
            var query = new Query<Account>()
                .AddFilters(new Filter().WhereNotEqual(Account.Fields.Name, "test"));

            Assert.Equal(Account.Fields.Name, query.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.NotEqual, query.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal("test", query.QueryExpression.Criteria.Filters.First().Conditions.First().Values.First());

            var query2 = new Query<Account>()
                .AddFilters(new Filter().WhereNotEqual(Account.Fields.Name, "test", Account.EntityLogicalName));

            Assert.Equal(Account.EntityLogicalName, query2.QueryExpression.Criteria.Filters.First().Conditions.First().EntityName);
            Assert.Equal(Account.Fields.Name, query2.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.NotEqual, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal("test", query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values.First());
        }

        [Fact]
        public void ShouldSetWhereNotEqualBusinessId()
        {
            var query = new Query<Account>()
                .AddFilters(new Filter().WhereNotEqualBusinessId(Account.Fields.OwningBusinessUnit));

            Assert.Equal(Account.Fields.OwningBusinessUnit, query.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.NotEqualBusinessId, query.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Empty(query.QueryExpression.Criteria.Filters.First().Conditions.First().Values);

            var query2 = new Query<Account>()
                .AddFilters(new Filter().WhereNotEqualBusinessId(Account.Fields.OwningBusinessUnit, Account.EntityLogicalName));

            Assert.Equal(Account.EntityLogicalName, query2.QueryExpression.Criteria.Filters.First().Conditions.First().EntityName);
            Assert.Equal(Account.Fields.OwningBusinessUnit, query2.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.NotEqualBusinessId, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Empty(query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereNotEqualUserId()
        {
            var query = new Query<Account>()
                .AddFilters(new Filter().WhereNotEqualUserId(Account.Fields.OwnerId));

            Assert.Equal(Account.Fields.OwnerId, query.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.NotEqualUserId, query.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Empty(query.QueryExpression.Criteria.Filters.First().Conditions.First().Values);

            var query2 = new Query<Account>()
                .AddFilters(new Filter().WhereNotEqualUserId(Account.Fields.OwnerId, Account.EntityLogicalName));

            Assert.Equal(Account.EntityLogicalName, query2.QueryExpression.Criteria.Filters.First().Conditions.First().EntityName);
            Assert.Equal(Account.Fields.OwnerId, query2.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.NotEqualUserId, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Empty(query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereNotIn()
        {
            var query = new Query<Account>()
                .AddFilters(new Filter().WhereNotIn(Account.Fields.CustomerTypeCode, 1, 2, 3));

            Assert.Equal(Account.Fields.CustomerTypeCode, query.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.NotIn, query.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Contains(1, query.QueryExpression.Criteria.Filters.First().Conditions.First().Values);
            Assert.Contains(2, query.QueryExpression.Criteria.Filters.First().Conditions.First().Values);
            Assert.Contains(3, query.QueryExpression.Criteria.Filters.First().Conditions.First().Values);

            var query2 = new Query<Account>()
                .AddFilters(new Filter().WhereNotIn(Account.EntityLogicalName, Account.Fields.CustomerTypeCode, 1, 2, 3));

            Assert.Equal(Account.EntityLogicalName, query2.QueryExpression.Criteria.Filters.First().Conditions.First().EntityName);
            Assert.Equal(Account.Fields.CustomerTypeCode, query2.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.NotIn, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Contains(1, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values);
            Assert.Contains(2, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values);
            Assert.Contains(3, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereNotInList()
        {
            var query = new Query<Account>()
                .AddFilters(new Filter().WhereNotIn(Account.Fields.CustomerTypeCode, new List<int> { 1, 2, 3 }));

            Assert.Equal(Account.Fields.CustomerTypeCode, query.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.NotIn, query.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.IsAssignableFrom<IList>(query.QueryExpression.Criteria.Filters.First().Conditions.First().Values);

            var query2 = new Query<Account>()
                .AddFilters(new Filter().WhereNotIn(Account.EntityLogicalName, Account.Fields.CustomerTypeCode, new List<int> { 1, 2, 3 }));

            Assert.Equal(Account.EntityLogicalName, query2.QueryExpression.Criteria.Filters.First().Conditions.First().EntityName);
            Assert.Equal(Account.Fields.CustomerTypeCode, query2.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.NotIn, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.IsAssignableFrom<IList>(query.QueryExpression.Criteria.Filters.First().Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereNotLike()
        {
            var query = new Query<Account>()
                .AddFilters(new Filter().WhereNotLike(Account.Fields.Name, "%test%"));

            Assert.Equal(Account.Fields.Name, query.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.NotLike, query.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal("%test%", query.QueryExpression.Criteria.Filters.First().Conditions.First().Values.First());

            var query2 = new Query<Account>()
                .AddFilters(new Filter().WhereNotLike(Account.Fields.Name, "%test%", Account.EntityLogicalName));

            Assert.Equal(Account.EntityLogicalName, query2.QueryExpression.Criteria.Filters.First().Conditions.First().EntityName);
            Assert.Equal(Account.Fields.Name, query2.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.NotLike, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal("%test%", query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values.First());
        }

        [Fact]
        public void ShouldSetWhereNotMask()
        {
            var obj = new object();
            var query = new Query<Account>()
                .AddFilters(new Filter().WhereNotMask(Account.Fields.Name, obj));

            Assert.Equal(Account.Fields.Name, query.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.NotMask, query.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal(query.QueryExpression.Criteria.Filters.First().Conditions.First().Values.First(), obj);

            var query2 = new Query<Account>()
                 .AddFilters(new Filter().WhereNotMask(Account.Fields.Name, obj, Account.EntityLogicalName));

            Assert.Equal(Account.EntityLogicalName, query2.QueryExpression.Criteria.Filters.First().Conditions.First().EntityName);
            Assert.Equal(Account.Fields.Name, query2.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.NotMask, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal(query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values.First(), obj);
        }

        [Fact]
        public void ShouldSetWhereNotNull()
        {
            var query = new Query<Account>()
                .AddFilters(new Filter().WhereNotNull(Account.Fields.Name));

            Assert.Equal(Account.Fields.Name, query.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.NotNull, query.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Empty(query.QueryExpression.Criteria.Filters.First().Conditions.First().Values);

            var query2 = new Query<Account>()
                .AddFilters(new Filter().WhereNotNull(Account.Fields.Name, Account.EntityLogicalName));

            Assert.Equal(Account.EntityLogicalName, query2.QueryExpression.Criteria.Filters.First().Conditions.First().EntityName);
            Assert.Equal(Account.Fields.Name, query2.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.NotNull, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Empty(query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereNotOn()
        {
            var date = new DateTime();
            var query = new Query<Account>()
                .AddFilters(new Filter().WhereNotOn(Account.Fields.CreatedOn, date));

            Assert.Equal(Account.Fields.CreatedOn, query.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.NotOn, query.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal(date, query.QueryExpression.Criteria.Filters.First().Conditions.First().Values.First());

            var query2 = new Query<Account>()
                .AddFilters(new Filter().WhereNotOn(Account.Fields.CreatedOn, date, Account.EntityLogicalName));

            Assert.Equal(Account.EntityLogicalName, query2.QueryExpression.Criteria.Filters.First().Conditions.First().EntityName);
            Assert.Equal(Account.Fields.CreatedOn, query2.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.NotOn, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal(date, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values.First());
        }

        [Fact]
        public void ShouldSetWhereNotUnder()
        {
            var guid = new Guid();
            var query = new Query<Account>()
                .AddFilters(new Filter().WhereNotUnder(Account.Fields.AccountId, guid));

            Assert.Equal(Account.Fields.AccountId, query.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.NotUnder, query.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal(guid, query.QueryExpression.Criteria.Filters.First().Conditions.First().Values.First());

            var query2 = new Query<Account>()
                .AddFilters(new Filter().WhereNotUnder(Account.Fields.AccountId, guid, Account.EntityLogicalName));

            Assert.Equal(Account.EntityLogicalName, query2.QueryExpression.Criteria.Filters.First().Conditions.First().EntityName);
            Assert.Equal(Account.Fields.AccountId, query2.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.NotUnder, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal(guid, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values.First());
        }

        [Fact]
        public void ShouldSetWhereNull()
        {
            var query = new Query<Account>()
                .AddFilters(new Filter().WhereNull(Account.Fields.Name));

            Assert.Equal(Account.Fields.Name, query.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.Null, query.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Empty(query.QueryExpression.Criteria.Filters.First().Conditions.First().Values);

            var query2 = new Query<Account>()
                .AddFilters(new Filter().WhereNull(Account.Fields.Name, Account.EntityLogicalName));

            Assert.Equal(Account.EntityLogicalName, query2.QueryExpression.Criteria.Filters.First().Conditions.First().EntityName);
            Assert.Equal(Account.Fields.Name, query2.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.Null, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Empty(query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereOlderThanXDays()
        {
            var query = new Query<Account>()
                .AddFilters(new Filter().WhereOlderThanXDays(Account.Fields.CreatedOn, 10));

            Assert.Equal(Account.Fields.CreatedOn, query.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.OlderThanXDays, query.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal(10, query.QueryExpression.Criteria.Filters.First().Conditions.First().Values.First());

            var query2 = new Query<Account>()
                .AddFilters(new Filter().WhereOlderThanXDays(Account.Fields.CreatedOn, 10, Account.EntityLogicalName));

            Assert.Equal(Account.EntityLogicalName, query2.QueryExpression.Criteria.Filters.First().Conditions.First().EntityName);
            Assert.Equal(Account.Fields.CreatedOn, query2.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.OlderThanXDays, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal(10, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values.First());
        }

        [Fact]
        public void ShouldSetWhereOlderThanXHours()
        {
            var query = new Query<Account>()
                .AddFilters(new Filter().WhereOlderThanXHours(Account.Fields.CreatedOn, 10));

            Assert.Equal(Account.Fields.CreatedOn, query.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.OlderThanXHours, query.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal(10, query.QueryExpression.Criteria.Filters.First().Conditions.First().Values.First());

            var query2 = new Query<Account>()
                .AddFilters(new Filter().WhereOlderThanXHours(Account.Fields.CreatedOn, 10, Account.EntityLogicalName));

            Assert.Equal(Account.EntityLogicalName, query2.QueryExpression.Criteria.Filters.First().Conditions.First().EntityName);
            Assert.Equal(Account.Fields.CreatedOn, query2.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.OlderThanXHours, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal(10, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values.First());
        }

        [Fact]
        public void ShouldSetWhereOlderThanXMinutes()
        {
            var query = new Query<Account>()
                .AddFilters(new Filter().WhereOlderThanXMinutes(Account.Fields.CreatedOn, 10));

            Assert.Equal(Account.Fields.CreatedOn, query.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.OlderThanXMinutes, query.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal(10, query.QueryExpression.Criteria.Filters.First().Conditions.First().Values.First());

            var query2 = new Query<Account>()
                .AddFilters(new Filter().WhereOlderThanXMinutes(Account.Fields.CreatedOn, 10, Account.EntityLogicalName));

            Assert.Equal(Account.EntityLogicalName, query2.QueryExpression.Criteria.Filters.First().Conditions.First().EntityName);
            Assert.Equal(Account.Fields.CreatedOn, query2.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.OlderThanXMinutes, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal(10, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values.First());
        }

        [Fact]
        public void ShouldSetWhereOlderThanXMonths()
        {
            var query = new Query<Account>()
                .AddFilters(new Filter().WhereOlderThanXMonths(Account.Fields.CreatedOn, 10));

            Assert.Equal(Account.Fields.CreatedOn, query.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.OlderThanXMonths, query.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal(10, query.QueryExpression.Criteria.Filters.First().Conditions.First().Values.First());

            var query2 = new Query<Account>()
                .AddFilters(new Filter().WhereOlderThanXMonths(Account.Fields.CreatedOn, 10, Account.EntityLogicalName));

            Assert.Equal(Account.EntityLogicalName, query2.QueryExpression.Criteria.Filters.First().Conditions.First().EntityName);
            Assert.Equal(Account.Fields.CreatedOn, query2.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.OlderThanXMonths, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal(10, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values.First());
        }

        [Fact]
        public void ShouldSetWhereOlderThanXWeeks()
        {
            var query = new Query<Account>()
                .AddFilters(new Filter().WhereOlderThanXWeeks(Account.Fields.CreatedOn, 10));

            Assert.Equal(Account.Fields.CreatedOn, query.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.OlderThanXWeeks, query.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal(10, query.QueryExpression.Criteria.Filters.First().Conditions.First().Values.First());

            var query2 = new Query<Account>()
                .AddFilters(new Filter().WhereOlderThanXWeeks(Account.Fields.CreatedOn, 10, Account.EntityLogicalName));

            Assert.Equal(Account.EntityLogicalName, query2.QueryExpression.Criteria.Filters.First().Conditions.First().EntityName);
            Assert.Equal(Account.Fields.CreatedOn, query2.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.OlderThanXWeeks, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal(10, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values.First());
        }

        [Fact]
        public void ShouldSetWhereOlderThanXYears()
        {
            var query = new Query<Account>()
                .AddFilters(new Filter().WhereOlderThanXYears(Account.Fields.CreatedOn, 10));

            Assert.Equal(Account.Fields.CreatedOn, query.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.OlderThanXYears, query.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal(10, query.QueryExpression.Criteria.Filters.First().Conditions.First().Values.First());

            var query2 = new Query<Account>()
                .AddFilters(new Filter().WhereOlderThanXYears(Account.Fields.CreatedOn, 10, Account.EntityLogicalName));

            Assert.Equal(Account.EntityLogicalName, query2.QueryExpression.Criteria.Filters.First().Conditions.First().EntityName);
            Assert.Equal(Account.Fields.CreatedOn, query2.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.OlderThanXYears, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal(10, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values.First());
        }

        [Fact]
        public void ShouldSetWhereOn()
        {
            var date = new DateTime();
            var query = new Query<Account>()
                .AddFilters(new Filter().WhereOn(Account.Fields.CreatedOn, date));

            Assert.Equal(Account.Fields.CreatedOn, query.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.On, query.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal(date, query.QueryExpression.Criteria.Filters.First().Conditions.First().Values.First());

            var query2 = new Query<Account>()
                .AddFilters(new Filter().WhereOn(Account.Fields.CreatedOn, date, Account.EntityLogicalName));

            Assert.Equal(Account.EntityLogicalName, query2.QueryExpression.Criteria.Filters.First().Conditions.First().EntityName);
            Assert.Equal(Account.Fields.CreatedOn, query2.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.On, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal(date, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values.First());
        }

        [Fact]
        public void ShouldSetWhereOnOrAfter()
        {
            var date = new DateTime();
            var query = new Query<Account>()
                .AddFilters(new Filter().WhereOnOrAfter(Account.Fields.CreatedOn, date));

            Assert.Equal(Account.Fields.CreatedOn, query.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.OnOrAfter, query.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal(date, query.QueryExpression.Criteria.Filters.First().Conditions.First().Values.First());

            var query2 = new Query<Account>()
                .AddFilters(new Filter().WhereOnOrAfter(Account.Fields.CreatedOn, date, Account.EntityLogicalName));

            Assert.Equal(Account.EntityLogicalName, query2.QueryExpression.Criteria.Filters.First().Conditions.First().EntityName);
            Assert.Equal(Account.Fields.CreatedOn, query2.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.OnOrAfter, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal(date, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values.First());
        }

        [Fact]
        public void ShouldSetWhereOnOrBefore()
        {
            var date = new DateTime();
            var query = new Query<Account>()
                .AddFilters(new Filter().WhereOnOrBefore(Account.Fields.CreatedOn, date));

            Assert.Equal(Account.Fields.CreatedOn, query.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.OnOrBefore, query.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal(date, query.QueryExpression.Criteria.Filters.First().Conditions.First().Values.First());

            var query2 = new Query<Account>()
                .AddFilters(new Filter().WhereOnOrBefore(Account.Fields.CreatedOn, date, Account.EntityLogicalName));

            Assert.Equal(Account.EntityLogicalName, query2.QueryExpression.Criteria.Filters.First().Conditions.First().EntityName);
            Assert.Equal(Account.Fields.CreatedOn, query2.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.OnOrBefore, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal(date, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values.First());
        }

        [Fact]
        public void ShouldSetWhereThisFiscalPeriod()
        {
            var query = new Query<Account>()
                .AddFilters(new Filter().WhereThisFiscalPeriod(Account.Fields.CreatedOn));

            Assert.Equal(Account.Fields.CreatedOn, query.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.ThisFiscalPeriod, query.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Empty(query.QueryExpression.Criteria.Filters.First().Conditions.First().Values);

            var query2 = new Query<Account>()
                .AddFilters(new Filter().WhereThisFiscalPeriod(Account.Fields.CreatedOn, Account.EntityLogicalName));

            Assert.Equal(Account.EntityLogicalName, query2.QueryExpression.Criteria.Filters.First().Conditions.First().EntityName);
            Assert.Equal(Account.Fields.CreatedOn, query2.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.ThisFiscalPeriod, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Empty(query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereThisFiscalYear()
        {
            var query = new Query<Account>()
                .AddFilters(new Filter().WhereThisFiscalYear(Account.Fields.CreatedOn));

            Assert.Equal(Account.Fields.CreatedOn, query.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.ThisFiscalYear, query.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Empty(query.QueryExpression.Criteria.Filters.First().Conditions.First().Values);

            var query2 = new Query<Account>()
                .AddFilters(new Filter().WhereThisFiscalYear(Account.Fields.CreatedOn, Account.EntityLogicalName));

            Assert.Equal(Account.EntityLogicalName, query2.QueryExpression.Criteria.Filters.First().Conditions.First().EntityName);
            Assert.Equal(Account.Fields.CreatedOn, query2.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.ThisFiscalYear, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Empty(query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereThisMonth()
        {
            var query = new Query<Account>()
                .AddFilters(new Filter().WhereThisMonth(Account.Fields.CreatedOn));

            Assert.Equal(Account.Fields.CreatedOn, query.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.ThisMonth, query.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Empty(query.QueryExpression.Criteria.Filters.First().Conditions.First().Values);

            var query2 = new Query<Account>()
                .AddFilters(new Filter().WhereThisMonth(Account.Fields.CreatedOn, Account.EntityLogicalName));

            Assert.Equal(Account.EntityLogicalName, query2.QueryExpression.Criteria.Filters.First().Conditions.First().EntityName);
            Assert.Equal(Account.Fields.CreatedOn, query2.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.ThisMonth, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Empty(query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereThisWeek()
        {
            var query = new Query<Account>()
                .AddFilters(new Filter().WhereThisWeek(Account.Fields.CreatedOn));

            Assert.Equal(Account.Fields.CreatedOn, query.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.ThisWeek, query.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Empty(query.QueryExpression.Criteria.Filters.First().Conditions.First().Values);

            var query2 = new Query<Account>()
                .AddFilters(new Filter().WhereThisWeek(Account.Fields.CreatedOn, Account.EntityLogicalName));

            Assert.Equal(Account.EntityLogicalName, query2.QueryExpression.Criteria.Filters.First().Conditions.First().EntityName);
            Assert.Equal(Account.Fields.CreatedOn, query2.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.ThisWeek, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Empty(query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereThisYear()
        {
            var query = new Query<Account>()
                .AddFilters(new Filter().WhereThisYear(Account.Fields.CreatedOn));

            Assert.Equal(Account.Fields.CreatedOn, query.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.ThisYear, query.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Empty(query.QueryExpression.Criteria.Filters.First().Conditions.First().Values);

            var query2 = new Query<Account>()
                .AddFilters(new Filter().WhereThisYear(Account.Fields.CreatedOn, Account.EntityLogicalName));

            Assert.Equal(Account.EntityLogicalName, query2.QueryExpression.Criteria.Filters.First().Conditions.First().EntityName);
            Assert.Equal(Account.Fields.CreatedOn, query2.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.ThisYear, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Empty(query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereToday()
        {
            var query = new Query<Account>()
                .AddFilters(new Filter().WhereToday(Account.Fields.CreatedOn));

            Assert.Equal(Account.Fields.CreatedOn, query.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.Today, query.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Empty(query.QueryExpression.Criteria.Filters.First().Conditions.First().Values);

            var query2 = new Query<Account>()
                .AddFilters(new Filter().WhereToday(Account.Fields.CreatedOn, Account.EntityLogicalName));

            Assert.Equal(Account.EntityLogicalName, query2.QueryExpression.Criteria.Filters.First().Conditions.First().EntityName);
            Assert.Equal(Account.Fields.CreatedOn, query2.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.Today, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Empty(query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereTomorrow()
        {
            var query = new Query<Account>()
                .AddFilters(new Filter().WhereTomorrow(Account.Fields.CreatedOn));

            Assert.Equal(Account.Fields.CreatedOn, query.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.Tomorrow, query.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Empty(query.QueryExpression.Criteria.Filters.First().Conditions.First().Values);

            var query2 = new Query<Account>()
                .AddFilters(new Filter().WhereTomorrow(Account.Fields.CreatedOn, Account.EntityLogicalName));

            Assert.Equal(Account.EntityLogicalName, query2.QueryExpression.Criteria.Filters.First().Conditions.First().EntityName);
            Assert.Equal(Account.Fields.CreatedOn, query2.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.Tomorrow, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Empty(query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereUnder()
        {
            var guid = new Guid();
            var query = new Query<Account>()
                .AddFilters(new Filter().WhereUnder(Account.Fields.AccountId, guid));

            Assert.Equal(Account.Fields.AccountId, query.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.Under, query.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal(guid, query.QueryExpression.Criteria.Filters.First().Conditions.First().Values.First());

            var query2 = new Query<Account>()
                .AddFilters(new Filter().WhereUnder(Account.Fields.AccountId, guid, Account.EntityLogicalName));

            Assert.Equal(Account.EntityLogicalName, query2.QueryExpression.Criteria.Filters.First().Conditions.First().EntityName);
            Assert.Equal(Account.Fields.AccountId, query2.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.Under, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal(guid, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values.First());
        }

        [Fact]
        public void ShouldSetWhereUnderOrEqual()
        {
            var guid = new Guid();
            var query = new Query<Account>()
                .AddFilters(new Filter().WhereUnderOrEqual(Account.Fields.AccountId, guid));

            Assert.Equal(Account.Fields.AccountId, query.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.UnderOrEqual, query.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal(guid, query.QueryExpression.Criteria.Filters.First().Conditions.First().Values.First());

            var query2 = new Query<Account>()
                .AddFilters(new Filter().WhereUnderOrEqual(Account.Fields.AccountId, guid, Account.EntityLogicalName));

            Assert.Equal(Account.EntityLogicalName, query2.QueryExpression.Criteria.Filters.First().Conditions.First().EntityName);
            Assert.Equal(Account.Fields.AccountId, query2.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.UnderOrEqual, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Equal(guid, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values.First());
        }

        [Fact]
        public void ShouldSetWhereYesterday()
        {
            var query = new Query<Account>()
                .AddFilters(new Filter().WhereYesterday(Account.Fields.CreatedOn));

            Assert.Equal(Account.Fields.CreatedOn, query.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.Yesterday, query.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Empty(query.QueryExpression.Criteria.Filters.First().Conditions.First().Values);

            var query2 = new Query<Account>()
                .AddFilters(new Filter().WhereYesterday(Account.Fields.CreatedOn, Account.EntityLogicalName));

            Assert.Equal(Account.EntityLogicalName, query2.QueryExpression.Criteria.Filters.First().Conditions.First().EntityName);
            Assert.Equal(Account.Fields.CreatedOn, query2.QueryExpression.Criteria.Filters.First().Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.Yesterday, query2.QueryExpression.Criteria.Filters.First().Conditions.First().Operator);
            Assert.Empty(query2.QueryExpression.Criteria.Filters.First().Conditions.First().Values);
        }

        #endregion Conditions
    }
}