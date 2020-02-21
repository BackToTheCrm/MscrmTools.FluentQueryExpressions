using FakeXrmEasy;
using FluentAssertions;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using MscrmTools.FluentQueryExpressions.Test.AppCode;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace MscrmTools.FluentQueryExpressions.Test
{
    public class QueryExpressionTest
    {
        [Fact]
        public void ShouldAddAllAttributes()
        {
            var query = new Query<Account>(null)
                .Select(true);

            Assert.True(query.QueryExpression.ColumnSet.AllColumns);
        }

        [Fact]
        public void ShouldAddAttributesWithAnonymousType()
        {
            var query = new Query<Account>(null)
                .Select(a => new { a.Name, a.AccountNumber });

            Assert.Contains(Account.Fields.Name, query.QueryExpression.ColumnSet.Columns);
            Assert.Contains(Account.Fields.AccountNumber, query.QueryExpression.ColumnSet.Columns);
        }

        [Fact]
        public void ShouldAddFilter()
        {
            var query = new Query<Account>(null)
                .AddFilters(LogicalOperator.Or, new Filter(LogicalOperator.Or));

            Assert.Equal(LogicalOperator.Or, query.QueryExpression.Criteria.FilterOperator);
            Assert.Single(query.QueryExpression.Criteria.Filters);
            Assert.Equal(LogicalOperator.Or, query.QueryExpression.Criteria.Filters.First().FilterOperator);
        }

        [Fact]
        public void ShouldAddFilterWithoutOperator()
        {
            var query = new Query<Account>(null)
                .AddFilters(new Filter(LogicalOperator.Or));

            Assert.Equal(LogicalOperator.And, query.QueryExpression.Criteria.FilterOperator);
            Assert.Single(query.QueryExpression.Criteria.Filters);
            Assert.Equal(LogicalOperator.Or, query.QueryExpression.Criteria.Filters.First().FilterOperator);
        }

        [Fact]
        public void ShouldAddLink()
        {
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId));

            Assert.Single(query.QueryExpression.LinkEntities);

            Assert.Equal(Contact.EntityLogicalName, query.QueryExpression.LinkEntities.First().LinkToEntityName);
            Assert.Equal(Contact.Fields.ParentCustomerId, query.QueryExpression.LinkEntities.First().LinkToAttributeName);
            Assert.Equal(Account.Fields.AccountId, query.QueryExpression.LinkEntities.First().LinkFromAttributeName);
            Assert.Equal(Account.EntityLogicalName, query.QueryExpression.LinkEntities.First().LinkFromEntityName);
            Assert.Equal(Contact.EntityLogicalName, query.QueryExpression.LinkEntities.First().EntityAlias);
        }

        [Fact]
        public void ShouldAddLinks()
        {
            var query = new Query<Account>(null)
                        .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId))
                        .AddLink(new Link<Task>(Task.Fields.RegardingObjectId, Account.Fields.AccountId));

            Assert.Equal(2, query.QueryExpression.LinkEntities.Count);
            Assert.Equal(Contact.EntityLogicalName, query.QueryExpression.LinkEntities[0].LinkToEntityName);
            Assert.Equal(Contact.Fields.ParentCustomerId, query.QueryExpression.LinkEntities[0].LinkToAttributeName);
            Assert.Equal(Account.Fields.AccountId, query.QueryExpression.LinkEntities[0].LinkFromAttributeName);
            Assert.Equal(Account.EntityLogicalName, query.QueryExpression.LinkEntities[0].LinkFromEntityName);
            Assert.Equal(Contact.EntityLogicalName, query.QueryExpression.LinkEntities[0].EntityAlias);
            Assert.Equal(Task.EntityLogicalName, query.QueryExpression.LinkEntities[1].LinkToEntityName);
            Assert.Equal(Task.Fields.RegardingObjectId, query.QueryExpression.LinkEntities[1].LinkToAttributeName);
            Assert.Equal(Account.Fields.AccountId, query.QueryExpression.LinkEntities[1].LinkFromAttributeName);
            Assert.Equal(Account.EntityLogicalName, query.QueryExpression.LinkEntities[1].LinkFromEntityName);
            Assert.Equal(Task.EntityLogicalName, query.QueryExpression.LinkEntities[1].EntityAlias);
        }

        [Fact]
        public void ShouldAddNoAttribute()
        {
            var query = new Query<Account>(null)
                .Select();

            Assert.False(query.QueryExpression.ColumnSet.AllColumns);
        }

        [Fact]
        public void ShouldAddOneAttribute()
        {
            var query = new Query<Account>(null)
                .Select(Account.Fields.Name);

            Assert.Contains(Account.Fields.Name, query.QueryExpression.ColumnSet.Columns);
        }

        [Fact]
        public void ShouldAddOrder()
        {
            var query = new Query<Account>(null)
                .Order(Account.Fields.Name, OrderType.Ascending);

            Assert.Equal(query.QueryExpression.Orders.First().AttributeName, Account.Fields.Name);
            Assert.Equal(OrderType.Ascending, query.QueryExpression.Orders.First().OrderType);
        }

        [Fact]
        public void ShouldAddPaging()
        {
            var query = new Query<Account>(null)
                .SetPagingInfo(1, 100, true);

            Assert.Equal(1, query.QueryExpression.PageInfo.PageNumber);
            Assert.Equal(100, query.QueryExpression.PageInfo.Count);
            Assert.True(query.QueryExpression.PageInfo.ReturnTotalRecordCount);
        }

        [Fact]
        public void ShouldAddTwoAttributes()
        {
            var query = new Query<Account>(null)
                        .Select(Account.Fields.Name)
                        .Select(Account.Fields.AccountNumber);

            Assert.Contains(Account.Fields.Name, query.QueryExpression.ColumnSet.Columns);
            Assert.Contains(Account.Fields.AccountNumber, query.QueryExpression.ColumnSet.Columns);
        }

        [Fact]
        public void ShouldAddTwoFilters()
        {
            var query = new Query<Account>(null)
                .AddFilters(
                    LogicalOperator.Or,
                    new Filter(LogicalOperator.Or),
                    new Filter()
                );

            Assert.Equal(LogicalOperator.Or, query.QueryExpression.Criteria.FilterOperator);
            Assert.Equal(2, query.QueryExpression.Criteria.Filters.Count);
            Assert.Equal(LogicalOperator.Or, query.QueryExpression.Criteria.Filters[0].FilterOperator);
            Assert.Equal(LogicalOperator.And, query.QueryExpression.Criteria.Filters[1].FilterOperator);
        }

        [Fact]
        public void ShouldBeAccountQueryExpression()
        {
            var query = new Query<Account>(null);

            Assert.Equal(query.QueryExpression.EntityName, Account.EntityLogicalName);
        }

        [Fact]
        public void ShouldBeDistinct()
        {
            var query = new Query<Account>(null)
                .Distinct();

            Assert.True(query.QueryExpression.Distinct);
        }

        [Fact]
        public void ShouldCreateLateBound()
        {
            var query = new Query(Account.EntityLogicalName, null);

            Assert.Equal(query.QueryExpression.EntityName, Account.EntityLogicalName);
        }

        [Fact]
        public void ShouldHaveNoLock()
        {
            var query = new Query<Account>(null)
                .NoLock();

            Assert.True(query.QueryExpression.NoLock);
        }

        [Fact]
        public void ShouldSetLogicalOperatorOr()
        {
            var query = new Query<Account>(null)
                .SetDefaultFilterOperator(LogicalOperator.Or);

            Assert.Equal(LogicalOperator.Or, query.QueryExpression.Criteria.FilterOperator);
        }

        [Fact]
        public void ShouldSetNextPage()
        {
            var query = new Query<Account>(null)
                        .SetPagingInfo(1, 100, true)
                        .NextPage("<fakePagingCookie>");

            Assert.Equal(2, query.QueryExpression.PageInfo.PageNumber);
            Assert.Equal(100, query.QueryExpression.PageInfo.Count);
            Assert.True(query.QueryExpression.PageInfo.ReturnTotalRecordCount);
            Assert.Equal("<fakePagingCookie>", query.QueryExpression.PageInfo.PagingCookie);
        }

        [Fact]
        public void ShouldSetTop()
        {
            var query = new Query<Account>(null)
                .Top(100);

            Assert.Equal(100, query.QueryExpression.TopCount);
        }

        #region Conditions

        [Fact]
        public void ShouldSetWhere()
        {
            var guid = Guid.NewGuid();
            var query = new Query<Account>(null)
                .Where(Account.Fields.AccountId, ConditionOperator.Above, guid);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.AccountId);
            Assert.Equal(ConditionOperator.Above, query.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.Single(), guid);

            var query2 = new Query<Account>(null)
                .Where(
                    Account.EntityLogicalName,
                    Account.Fields.AccountId,
                    ConditionOperator.Above,
                    guid
                );

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.AccountId);
            Assert.Equal(ConditionOperator.Above, query2.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.Single(), guid);
        }

        [Fact]
        public void ShouldSetWhereAbove()
        {
            var guid = Guid.NewGuid();
            var query = new Query<Account>(null)
                .WhereAbove(Account.Fields.AccountId, guid);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.AccountId);
            Assert.Equal(ConditionOperator.Above, query.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.Single(), guid);

            var query2 = new Query<Account>(null)
                .WhereAbove(Account.Fields.AccountId, guid, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.AccountId);
            Assert.Equal(ConditionOperator.Above, query2.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.Single(), guid);
        }

        [Fact]
        public void ShouldSetWhereAboveOrEqual()
        {
            var guid = Guid.NewGuid();
            var query = new Query<Account>(null)
                .WhereAboveOrEqual(Account.Fields.AccountId, guid);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.AccountId);
            Assert.Equal(ConditionOperator.AboveOrEqual, query.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.Single(), guid);

            var query2 = new Query<Account>(null)
                .WhereAboveOrEqual(Account.Fields.AccountId, guid, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.AccountId);
            Assert.Equal(ConditionOperator.AboveOrEqual, query2.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.Single(), guid);
        }

        [Fact]
        public void ShouldSetWhereBeginsWith()
        {
            var query = new Query<Account>(null)
                .WhereBeginsWith(Account.Fields.Name, "test");

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(ConditionOperator.BeginsWith, query.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal("test", query.QueryExpression.Criteria.Conditions.First().Values.Single());

            var query2 = new Query<Account>(null)
                .WhereBeginsWith(Account.Fields.Name, "test", Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(ConditionOperator.BeginsWith, query2.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal("test", query2.QueryExpression.Criteria.Conditions.First().Values.Single());
        }

        [Fact]
        public void ShouldSetWhereBetween()
        {
            var query = new Query<Account>(null)
                .WhereBetween(Account.Fields.NumberOfEmployees, 10, 50);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.NumberOfEmployees);
            Assert.Equal(ConditionOperator.Between, query.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal(10, query.QueryExpression.Criteria.Conditions.First().Values[0]);
            Assert.Equal(50, query.QueryExpression.Criteria.Conditions.First().Values[1]);

            var query2 = new Query<Account>(null)
                .WhereBetween(
                    Account.Fields.NumberOfEmployees,
                    10,
                    50,
                    Account.EntityLogicalName
                );

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.NumberOfEmployees);
            Assert.Equal(ConditionOperator.Between, query2.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal(10, query2.QueryExpression.Criteria.Conditions.First().Values[0]);
            Assert.Equal(50, query2.QueryExpression.Criteria.Conditions.First().Values[1]);
        }

        [Fact]
        public void ShouldSetWhereChildOf()
        {
            var guid = Guid.NewGuid();
            var query = new Query<Account>(null)
                .WhereChildOf(Account.Fields.AccountId, guid);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.AccountId);
            Assert.Equal(ConditionOperator.ChildOf, query.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.Single(), guid);

            var query2 = new Query<Account>(null)
                .WhereChildOf(Account.Fields.AccountId, guid, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.AccountId);
            Assert.Equal(ConditionOperator.ChildOf, query2.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.Single(), guid);
        }

        [Fact]
        public void ShouldSetWhereContains()
        {
            var query = new Query<Account>(null)
                .WhereContains(Account.Fields.Name, "test");

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(ConditionOperator.Contains, query.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal("test", query.QueryExpression.Criteria.Conditions.First().Values.Single());

            var query2 = new Query<Account>(null)
                .WhereContains(Account.Fields.Name, "test", Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(ConditionOperator.Contains, query2.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal("test", query2.QueryExpression.Criteria.Conditions.First().Values.Single());
        }

        [Fact]
        public void ShouldSetWhereContainValues()
        {
            var query = new Query<Account>(null)
                .WhereContainValues(
                    Account.Fields.CustomerTypeCode,
                    1,
                    2,
                    3
                );

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CustomerTypeCode);
            Assert.Equal(ConditionOperator.ContainValues, query.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Contains(1, query.QueryExpression.Criteria.Conditions.First().Values);
            Assert.Contains(2, query.QueryExpression.Criteria.Conditions.First().Values);
            Assert.Contains(3, query.QueryExpression.Criteria.Conditions.First().Values);

            var query2 = new Query<Account>(null)
                .WhereContainValues(
                    Account.EntityLogicalName,
                    Account.Fields.CustomerTypeCode,
                    1,
                    2,
                    3
                );

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CustomerTypeCode);
            Assert.Equal(ConditionOperator.ContainValues, query2.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Contains(1, query2.QueryExpression.Criteria.Conditions.First().Values);
            Assert.Contains(2, query2.QueryExpression.Criteria.Conditions.First().Values);
            Assert.Contains(3, query2.QueryExpression.Criteria.Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereDoesNotBeginWith()
        {
            var query = new Query<Account>(null)
                .WhereDoesNotBeginWith(Account.Fields.Name, "test");

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(ConditionOperator.DoesNotBeginWith, query.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal("test", query.QueryExpression.Criteria.Conditions.First().Values.Single());

            var query2 = new Query<Account>(null)
                .WhereDoesNotBeginWith(Account.Fields.Name, "test", Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(ConditionOperator.DoesNotBeginWith, query2.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal("test", query2.QueryExpression.Criteria.Conditions.First().Values.Single());
        }

        [Fact]
        public void ShouldSetWhereDoesNotContain()
        {
            var query = new Query<Account>(null)
                .WhereDoesNotContain(Account.Fields.Name, "test");

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(ConditionOperator.DoesNotContain, query.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal("test", query.QueryExpression.Criteria.Conditions.First().Values.Single());

            var query2 = new Query<Account>(null)
                .WhereDoesNotContain(Account.Fields.Name, "test", Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(ConditionOperator.DoesNotContain, query2.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal("test", query2.QueryExpression.Criteria.Conditions.First().Values.Single());
        }

        [Fact]
        public void ShouldSetWhereDoesNotContainValues()
        {
            var query = new Query<Account>(null)
                .WhereDoesNotContainValues(
                    Account.Fields.CustomerTypeCode,
                    1,
                    2,
                    3
                );

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CustomerTypeCode);
            Assert.Equal(ConditionOperator.DoesNotContainValues, query.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Contains(1, query.QueryExpression.Criteria.Conditions.First().Values);
            Assert.Contains(2, query.QueryExpression.Criteria.Conditions.First().Values);
            Assert.Contains(3, query.QueryExpression.Criteria.Conditions.First().Values);

            var query2 = new Query<Account>(null)
                .WhereDoesNotContainValues(
                    Account.EntityLogicalName,
                    Account.Fields.CustomerTypeCode,
                    1,
                    2,
                    3
                );

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CustomerTypeCode);
            Assert.Equal(ConditionOperator.DoesNotContainValues, query2.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Contains(1, query2.QueryExpression.Criteria.Conditions.First().Values);
            Assert.Contains(2, query2.QueryExpression.Criteria.Conditions.First().Values);
            Assert.Contains(3, query2.QueryExpression.Criteria.Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereDoesNotEndWith()
        {
            var query = new Query<Account>(null)
                .WhereDoesNotEndWith(Account.Fields.Name, "test");

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(ConditionOperator.DoesNotEndWith, query.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal("test", query.QueryExpression.Criteria.Conditions.First().Values.Single());

            var query2 = new Query<Account>(null)
                .WhereDoesNotEndWith(Account.Fields.Name, "test", Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(ConditionOperator.DoesNotEndWith, query2.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal("test", query2.QueryExpression.Criteria.Conditions.First().Values.Single());
        }

        [Fact]
        public void ShouldSetWhereEndsWith()
        {
            var query = new Query<Account>(null)
                .WhereEndsWith(Account.Fields.Name, "test");

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(ConditionOperator.EndsWith, query.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal("test", query.QueryExpression.Criteria.Conditions.First().Values.Single());

            var query2 = new Query<Account>(null)
                .WhereEndsWith(Account.Fields.Name, "test", Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(ConditionOperator.EndsWith, query2.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal("test", query2.QueryExpression.Criteria.Conditions.First().Values.Single());
        }

        [Fact]
        public void ShouldSetWhereEqual()
        {
            var query = new Query<Account>(null)
                .WhereEqual(Account.Fields.Name, "test");

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(ConditionOperator.Equal, query.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal("test", query.QueryExpression.Criteria.Conditions.First().Values.Single());

            var query2 = new Query<Account>(null)
                .WhereEqual(Account.Fields.Name, "test", Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(ConditionOperator.Equal, query2.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal("test", query2.QueryExpression.Criteria.Conditions.First().Values.Single());
        }

        [Fact]
        public void ShouldSetWhereEqualBusinessId()
        {
            var query = new Query<Account>(null)
                .WhereEqualBusinessId(Account.Fields.OwningBusinessUnit);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.OwningBusinessUnit);
            Assert.Equal(ConditionOperator.EqualBusinessId, query.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Empty(query.QueryExpression.Criteria.Conditions.First().Values);

            var query2 = new Query<Account>(null)
                .WhereEqualBusinessId(Account.Fields.OwningBusinessUnit, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.OwningBusinessUnit);
            Assert.Equal(ConditionOperator.EqualBusinessId, query2.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Empty(query2.QueryExpression.Criteria.Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereEqualUserId()
        {
            var query = new Query<Account>(null)
                .WhereEqualUserId(Account.Fields.OwnerId);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.OwnerId);
            Assert.Equal(ConditionOperator.EqualUserId, query.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Empty(query.QueryExpression.Criteria.Conditions.First().Values);

            var query2 = new Query<Account>(null)
                .WhereEqualUserId(Account.Fields.OwnerId, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.OwnerId);
            Assert.Equal(ConditionOperator.EqualUserId, query2.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Empty(query2.QueryExpression.Criteria.Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereEqualUserLanguage()
        {
            var query = new Query<Account>(null)
                .WhereEqualUserLanguage("no_language_attribute");

            Assert.Equal("no_language_attribute", query.QueryExpression.Criteria.Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.EqualUserLanguage, query.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Empty(query.QueryExpression.Criteria.Conditions.First().Values);

            var query2 = new Query<Account>(null)
                .WhereEqualUserLanguage("no_language_attribute", Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal("no_language_attribute", query2.QueryExpression.Criteria.Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.EqualUserLanguage, query2.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Empty(query2.QueryExpression.Criteria.Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereEqualUserOrUserHierarchy()
        {
            var query = new Query<Account>(null)
                .WhereEqualUserOrUserHierarchy(Account.Fields.OwnerId);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.OwnerId);
            Assert.Equal(ConditionOperator.EqualUserOrUserHierarchy, query.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Empty(query.QueryExpression.Criteria.Conditions.First().Values);

            var query2 = new Query<Account>(null)
                .WhereEqualUserOrUserHierarchy(Account.Fields.OwnerId, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.OwnerId);
            Assert.Equal(ConditionOperator.EqualUserOrUserHierarchy, query2.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Empty(query2.QueryExpression.Criteria.Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereEqualUserOrUserHierarchyAndTeams()
        {
            var query = new Query<Account>(null)
                .WhereEqualUserOrUserHierarchyAndTeams(Account.Fields.OwnerId);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.OwnerId);
            Assert.Equal(ConditionOperator.EqualUserOrUserHierarchyAndTeams, query.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Empty(query.QueryExpression.Criteria.Conditions.First().Values);

            var query2 = new Query<Account>(null)
                .WhereEqualUserOrUserHierarchyAndTeams(Account.Fields.OwnerId, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.OwnerId);
            Assert.Equal(ConditionOperator.EqualUserOrUserHierarchyAndTeams, query2.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Empty(query2.QueryExpression.Criteria.Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereEqualUserOrUserTeams()
        {
            var query = new Query<Account>(null)
                .WhereEqualUserOrUserTeams(Account.Fields.OwnerId);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.OwnerId);
            Assert.Equal(ConditionOperator.EqualUserOrUserTeams, query.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Empty(query.QueryExpression.Criteria.Conditions.First().Values);

            var query2 = new Query<Account>(null)
                .WhereEqualUserOrUserTeams(Account.Fields.OwnerId, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.OwnerId);
            Assert.Equal(ConditionOperator.EqualUserOrUserTeams, query2.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Empty(query2.QueryExpression.Criteria.Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereEqualUserTeams()
        {
            var query = new Query<Account>(null)
                .WhereEqualUserTeams(Account.Fields.OwnerId);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.OwnerId);
            Assert.Equal(ConditionOperator.EqualUserTeams, query.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Empty(query.QueryExpression.Criteria.Conditions.First().Values);

            var query2 = new Query<Account>(null)
                .WhereEqualUserTeams(Account.Fields.OwnerId, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.OwnerId);
            Assert.Equal(ConditionOperator.EqualUserTeams, query2.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Empty(query2.QueryExpression.Criteria.Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereGreaterEqual()
        {
            var query = new Query<Account>(null)
                .WhereGreaterEqual(Account.Fields.NumberOfEmployees, 10);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.NumberOfEmployees);
            Assert.Equal(ConditionOperator.GreaterEqual, query.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal(10, query.QueryExpression.Criteria.Conditions.First().Values.First());

            var query2 = new Query<Account>(null)
                .WhereGreaterEqual(Account.Fields.NumberOfEmployees, 10, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.NumberOfEmployees);
            Assert.Equal(ConditionOperator.GreaterEqual, query2.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal(10, query2.QueryExpression.Criteria.Conditions.First().Values.First());
        }

        [Fact]
        public void ShouldSetWhereGreaterThan()
        {
            var query = new Query<Account>(null)
                .WhereGreaterThan(Account.Fields.NumberOfEmployees, 10);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.NumberOfEmployees);
            Assert.Equal(ConditionOperator.GreaterThan, query.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal(10, query.QueryExpression.Criteria.Conditions.First().Values.First());

            var query2 = new Query<Account>(null)
                .WhereGreaterThan(Account.Fields.NumberOfEmployees, 10, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.NumberOfEmployees);
            Assert.Equal(ConditionOperator.GreaterThan, query2.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal(10, query2.QueryExpression.Criteria.Conditions.First().Values.First());
        }

        [Fact]
        public void ShouldSetWhereIn()
        {
            var query = new Query<Account>(null)
                .WhereIn(
                    Account.Fields.CustomerTypeCode,
                    1,
                    2,
                    3
                );

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CustomerTypeCode);
            Assert.Equal(ConditionOperator.In, query.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Contains(1, query.QueryExpression.Criteria.Conditions.First().Values);
            Assert.Contains(2, query.QueryExpression.Criteria.Conditions.First().Values);
            Assert.Contains(3, query.QueryExpression.Criteria.Conditions.First().Values);

            var query2 = new Query<Account>(null)
                .WhereIn(
                    Account.EntityLogicalName,
                    Account.Fields.CustomerTypeCode,
                    1,
                    2,
                    3
                );

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CustomerTypeCode);
            Assert.Equal(ConditionOperator.In, query2.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Contains(1, query2.QueryExpression.Criteria.Conditions.First().Values);
            Assert.Contains(2, query2.QueryExpression.Criteria.Conditions.First().Values);
            Assert.Contains(3, query2.QueryExpression.Criteria.Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereInFiscalPeriod()
        {
            var query = new Query<Account>(null)
                .WhereInFiscalPeriod(Account.Fields.CreatedOn, 1);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.InFiscalPeriod, query.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal(1, query.QueryExpression.Criteria.Conditions.First().Values.First());

            var query2 = new Query<Account>(null)
                .WhereInFiscalPeriod(Account.Fields.CreatedOn, 1, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.InFiscalPeriod, query2.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal(1, query2.QueryExpression.Criteria.Conditions.First().Values.First());
        }

        [Fact]
        public void ShouldSetWhereInFiscalPeriodAndYear()
        {
            var query = new Query<Account>(null)
                .WhereInFiscalPeriodAndYear(Account.Fields.CreatedOn, 1, 2018);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.InFiscalPeriodAndYear, query.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal(1, query.QueryExpression.Criteria.Conditions.First().Values[0]);
            Assert.Equal(2018, query.QueryExpression.Criteria.Conditions.First().Values[1]);

            var query2 = new Query<Account>(null)
                .WhereInFiscalPeriodAndYear(
                    Account.Fields.CreatedOn,
                    1,
                    2018,
                    Account.EntityLogicalName
                );

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.InFiscalPeriodAndYear, query2.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal(1, query2.QueryExpression.Criteria.Conditions.First().Values[0]);
            Assert.Equal(2018, query2.QueryExpression.Criteria.Conditions.First().Values[1]);
        }

        [Fact]
        public void ShouldSetWhereInFiscalYear()
        {
            var query = new Query<Account>(null)
                .WhereInFiscalYear(Account.Fields.CreatedOn, 2018);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.InFiscalYear, query.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal(2018, query.QueryExpression.Criteria.Conditions.First().Values.First());

            var query2 = new Query<Account>(null)
                .WhereInFiscalYear(Account.Fields.CreatedOn, 2018, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.InFiscalYear, query2.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal(2018, query2.QueryExpression.Criteria.Conditions.First().Values.First());
        }

        [Fact]
        public void ShouldSetWhereInList()
        {
            var query = new Query<Account>(null)
                .WhereIn(
                    Account.Fields.CustomerTypeCode,
                    new List<int>
                    {
                        1,
                        2,
                        3
                    }
                );

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CustomerTypeCode);
            Assert.Equal(ConditionOperator.In, query.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.IsAssignableFrom<IList>(query.QueryExpression.Criteria.Conditions.First().Values);

            var query2 = new Query<Account>(null)
                .WhereIn(
                    Account.EntityLogicalName,
                    Account.Fields.CustomerTypeCode,
                    new List<int>
                    {
                        1,
                        2,
                        3
                    }
                );

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CustomerTypeCode);
            Assert.Equal(ConditionOperator.In, query2.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.IsAssignableFrom<IList>(query.QueryExpression.Criteria.Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereInOrAfterFiscalPeriodAndYear()
        {
            var query = new Query<Account>(null)
                .WhereInOrAfterFiscalPeriodAndYear(Account.Fields.CreatedOn, 1, 2018);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.InOrAfterFiscalPeriodAndYear, query.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal(1, query.QueryExpression.Criteria.Conditions.First().Values[0]);
            Assert.Equal(2018, query.QueryExpression.Criteria.Conditions.First().Values[1]);

            var query2 = new Query<Account>(null)
                .WhereInOrAfterFiscalPeriodAndYear(
                    Account.Fields.CreatedOn,
                    1,
                    2018,
                    Account.EntityLogicalName
                );

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.InOrAfterFiscalPeriodAndYear, query2.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal(1, query2.QueryExpression.Criteria.Conditions.First().Values[0]);
            Assert.Equal(2018, query2.QueryExpression.Criteria.Conditions.First().Values[1]);
        }

        [Fact]
        public void ShouldSetWhereInOrBeforeFiscalPeriodAndYear()
        {
            var query = new Query<Account>(null)
                .WhereInOrBeforeFiscalPeriodAndYear(Account.Fields.CreatedOn, 1, 2018);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.InOrBeforeFiscalPeriodAndYear, query.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal(1, query.QueryExpression.Criteria.Conditions.First().Values[0]);
            Assert.Equal(2018, query.QueryExpression.Criteria.Conditions.First().Values[1]);

            var query2 = new Query<Account>(null)
                .WhereInOrBeforeFiscalPeriodAndYear(
                    Account.Fields.CreatedOn,
                    1,
                    2018,
                    Account.EntityLogicalName
                );

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.InOrBeforeFiscalPeriodAndYear, query2.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal(1, query2.QueryExpression.Criteria.Conditions.First().Values[0]);
            Assert.Equal(2018, query2.QueryExpression.Criteria.Conditions.First().Values[1]);
        }

        [Fact]
        public void ShouldSetWhereLast7Days()
        {
            var query = new Query<Account>(null)
                .WhereLast7Days(Account.Fields.CreatedOn);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.Last7Days, query.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Empty(query.QueryExpression.Criteria.Conditions.First().Values);

            var query2 = new Query<Account>(null)
                .WhereLast7Days(Account.Fields.CreatedOn, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.Last7Days, query2.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Empty(query2.QueryExpression.Criteria.Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereLastFiscalPeriod()
        {
            var query = new Query<Account>(null)
                .WhereLastFiscalPeriod(Account.Fields.CreatedOn);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.LastFiscalPeriod, query.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Empty(query.QueryExpression.Criteria.Conditions.First().Values);

            var query2 = new Query<Account>(null)
                .WhereLastFiscalPeriod(Account.Fields.CreatedOn, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.LastFiscalPeriod, query2.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Empty(query2.QueryExpression.Criteria.Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereLastFiscalYear()
        {
            var query = new Query<Account>(null)
                .WhereLastFiscalYear(Account.Fields.CreatedOn);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.LastFiscalYear, query.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Empty(query.QueryExpression.Criteria.Conditions.First().Values);

            var query2 = new Query<Account>(null)
                .WhereLastFiscalYear(Account.Fields.CreatedOn, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.LastFiscalYear, query2.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Empty(query2.QueryExpression.Criteria.Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereLastMonth()
        {
            var query = new Query<Account>(null)
                .WhereLastMonth(Account.Fields.CreatedOn);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.LastMonth, query.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Empty(query.QueryExpression.Criteria.Conditions.First().Values);

            var query2 = new Query<Account>(null)
                .WhereLastMonth(Account.Fields.CreatedOn, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.LastMonth, query2.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Empty(query2.QueryExpression.Criteria.Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereLastWeek()
        {
            var query = new Query<Account>(null)
                .WhereLastWeek(Account.Fields.CreatedOn);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.LastWeek, query.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Empty(query.QueryExpression.Criteria.Conditions.First().Values);

            var query2 = new Query<Account>(null)
                .WhereLastWeek(Account.Fields.CreatedOn, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.LastWeek, query2.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Empty(query2.QueryExpression.Criteria.Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereLastXDays()
        {
            var query = new Query<Account>(null)
                .WhereLastXDays(Account.Fields.CreatedOn, 10);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.LastXDays, query.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal(10, query.QueryExpression.Criteria.Conditions.First().Values.First());

            var query2 = new Query<Account>(null)
                .WhereLastXDays(Account.Fields.CreatedOn, 10, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.LastXDays, query2.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal(10, query2.QueryExpression.Criteria.Conditions.First().Values.First());
        }

        [Fact]
        public void ShouldSetWhereLastXFiscalPeriods()
        {
            var query = new Query<Account>(null)
                .WhereLastXFiscalPeriods(Account.Fields.CreatedOn, 10);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.LastXFiscalPeriods, query.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal(10, query.QueryExpression.Criteria.Conditions.First().Values.First());

            var query2 = new Query<Account>(null)
                .WhereLastXFiscalPeriods(Account.Fields.CreatedOn, 10, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.LastXFiscalPeriods, query2.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal(10, query2.QueryExpression.Criteria.Conditions.First().Values.First());
        }

        [Fact]
        public void ShouldSetWhereLastXFiscalYears()
        {
            var query = new Query<Account>(null)
                .WhereLastXFiscalYears(Account.Fields.CreatedOn, 10);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.LastXFiscalYears, query.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal(10, query.QueryExpression.Criteria.Conditions.First().Values.First());

            var query2 = new Query<Account>(null)
                .WhereLastXFiscalYears(Account.Fields.CreatedOn, 10, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.LastXFiscalYears, query2.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal(10, query2.QueryExpression.Criteria.Conditions.First().Values.First());
        }

        [Fact]
        public void ShouldSetWhereLastXHours()
        {
            var query = new Query<Account>(null)
                .WhereLastXHours(Account.Fields.CreatedOn, 10);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.LastXHours, query.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal(10, query.QueryExpression.Criteria.Conditions.First().Values.First());

            var query2 = new Query<Account>(null)
                .WhereLastXHours(Account.Fields.CreatedOn, 10, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.LastXHours, query2.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal(10, query2.QueryExpression.Criteria.Conditions.First().Values.First());
        }

        [Fact]
        public void ShouldSetWhereLastXMonths()
        {
            var query = new Query<Account>(null)
                .WhereLastXMonths(Account.Fields.CreatedOn, 10);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.LastXMonths, query.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal(10, query.QueryExpression.Criteria.Conditions.First().Values.First());

            var query2 = new Query<Account>(null)
                .WhereLastXMonths(Account.Fields.CreatedOn, 10, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.LastXMonths, query2.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal(10, query2.QueryExpression.Criteria.Conditions.First().Values.First());
        }

        [Fact]
        public void ShouldSetWhereLastXWeeks()
        {
            var query = new Query<Account>(null)
                .WhereLastXWeeks(Account.Fields.CreatedOn, 10);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.LastXWeeks, query.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal(10, query.QueryExpression.Criteria.Conditions.First().Values.First());

            var query2 = new Query<Account>(null)
                .WhereLastXWeeks(Account.Fields.CreatedOn, 10, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.LastXWeeks, query2.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal(10, query2.QueryExpression.Criteria.Conditions.First().Values.First());
        }

        [Fact]
        public void ShouldSetWhereLastXYears()
        {
            var query = new Query<Account>(null)
                .WhereLastXYears(Account.Fields.CreatedOn, 10);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.LastXYears, query.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal(10, query.QueryExpression.Criteria.Conditions.First().Values.First());

            var query2 = new Query<Account>(null)
                .WhereLastXYears(Account.Fields.CreatedOn, 10, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.LastXYears, query2.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal(10, query2.QueryExpression.Criteria.Conditions.First().Values.First());
        }

        [Fact]
        public void ShouldSetWhereLastYear()
        {
            var query = new Query<Account>(null)
                .WhereLastYear(Account.Fields.CreatedOn);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.LastYear, query.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Empty(query.QueryExpression.Criteria.Conditions.First().Values);

            var query2 = new Query<Account>(null)
                .WhereLastYear(Account.Fields.CreatedOn, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.LastYear, query2.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Empty(query2.QueryExpression.Criteria.Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereLessEqual()
        {
            var query = new Query<Account>(null)
                .WhereLessEqual(Account.Fields.NumberOfEmployees, 10);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.NumberOfEmployees);
            Assert.Equal(ConditionOperator.LessEqual, query.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal(10, query.QueryExpression.Criteria.Conditions.First().Values.First());

            var query2 = new Query<Account>(null)
                .WhereLessEqual(Account.Fields.NumberOfEmployees, 10, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.NumberOfEmployees);
            Assert.Equal(ConditionOperator.LessEqual, query2.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal(10, query2.QueryExpression.Criteria.Conditions.First().Values.First());
        }

        [Fact]
        public void ShouldSetWhereLessThan()
        {
            var query = new Query<Account>(null)
                .WhereLessThan(Account.Fields.NumberOfEmployees, 10);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.NumberOfEmployees);
            Assert.Equal(ConditionOperator.LessThan, query.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal(10, query.QueryExpression.Criteria.Conditions.First().Values.First());

            var query2 = new Query<Account>(null)
                .WhereLessThan(Account.Fields.NumberOfEmployees, 10, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.NumberOfEmployees);
            Assert.Equal(ConditionOperator.LessThan, query2.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal(10, query2.QueryExpression.Criteria.Conditions.First().Values.First());
        }

        [Fact]
        public void ShouldSetWhereLike()
        {
            var query = new Query<Account>(null)
                .WhereLike(Account.Fields.Name, "%test%");

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(ConditionOperator.Like, query.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal("%test%", query.QueryExpression.Criteria.Conditions.First().Values.First());

            var query2 = new Query<Account>(null)
                .WhereLike(Account.Fields.Name, "%test%", Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(ConditionOperator.Like, query2.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal("%test%", query2.QueryExpression.Criteria.Conditions.First().Values.First());
        }

        [Fact]
        public void ShouldSetWhereMask()
        {
            var obj = new object();
            var query = new Query<Account>(null)
                .WhereMask(Account.Fields.Name, obj);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(ConditionOperator.Mask, query.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.First(), obj);

            var query2 = new Query<Account>(null)
                .WhereMask(Account.Fields.Name, obj, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(ConditionOperator.Mask, query2.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.First(), obj);
        }

        [Fact]
        public void ShouldSetWhereMasksSelect()
        {
            var obj = new object();
            var query = new Query<Account>(null)
                .WhereMasksSelect(Account.Fields.Name, obj);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(ConditionOperator.MasksSelect, query.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.First(), obj);

            var query2 = new Query<Account>(null)
                .WhereMasksSelect(Account.Fields.Name, obj, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(ConditionOperator.MasksSelect, query2.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.First(), obj);
        }

        [Fact]
        public void ShouldSetWhereNext7Days()
        {
            var query = new Query<Account>(null)
                .WhereNext7Days(Account.Fields.CreatedOn);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.Next7Days, query.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Empty(query.QueryExpression.Criteria.Conditions.First().Values);

            var query2 = new Query<Account>(null)
                .WhereNext7Days(Account.Fields.CreatedOn, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.Next7Days, query2.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Empty(query2.QueryExpression.Criteria.Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereNextFiscalPeriod()
        {
            var query = new Query<Account>(null)
                .WhereNextFiscalPeriod(Account.Fields.CreatedOn);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.NextFiscalPeriod, query.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Empty(query.QueryExpression.Criteria.Conditions.First().Values);

            var query2 = new Query<Account>(null)
                .WhereNextFiscalPeriod(Account.Fields.CreatedOn, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.NextFiscalPeriod, query2.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Empty(query2.QueryExpression.Criteria.Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereNextFiscalYear()
        {
            var query = new Query<Account>(null)
                .WhereNextFiscalYear(Account.Fields.CreatedOn);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.NextFiscalYear, query.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Empty(query.QueryExpression.Criteria.Conditions.First().Values);

            var query2 = new Query<Account>(null)
                .WhereNextFiscalYear(Account.Fields.CreatedOn, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.NextFiscalYear, query2.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Empty(query2.QueryExpression.Criteria.Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereNextMonth()
        {
            var query = new Query<Account>(null)
                .WhereNextMonth(Account.Fields.CreatedOn);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.NextMonth, query.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Empty(query.QueryExpression.Criteria.Conditions.First().Values);

            var query2 = new Query<Account>(null)
                .WhereNextMonth(Account.Fields.CreatedOn, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.NextMonth, query2.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Empty(query2.QueryExpression.Criteria.Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereNextWeek()
        {
            var query = new Query<Account>(null)
                .WhereNextWeek(Account.Fields.CreatedOn);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.NextWeek, query.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Empty(query.QueryExpression.Criteria.Conditions.First().Values);

            var query2 = new Query<Account>(null)
                .WhereNextWeek(Account.Fields.CreatedOn, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.NextWeek, query2.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Empty(query2.QueryExpression.Criteria.Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereNextXDays()
        {
            var query = new Query<Account>(null)
                .WhereNextXDays(Account.Fields.CreatedOn, 10);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.NextXDays, query.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal(10, query.QueryExpression.Criteria.Conditions.First().Values.First());

            var query2 = new Query<Account>(null)
                .WhereNextXDays(Account.Fields.CreatedOn, 10, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.NextXDays, query2.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal(10, query2.QueryExpression.Criteria.Conditions.First().Values.First());
        }

        [Fact]
        public void ShouldSetWhereNextXFiscalPeriods()
        {
            var query = new Query<Account>(null)
                .WhereNextXFiscalPeriods(Account.Fields.CreatedOn, 10);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.NextXFiscalPeriods, query.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal(10, query.QueryExpression.Criteria.Conditions.First().Values.First());

            var query2 = new Query<Account>(null)
                .WhereNextXFiscalPeriods(Account.Fields.CreatedOn, 10, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.NextXFiscalPeriods, query2.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal(10, query2.QueryExpression.Criteria.Conditions.First().Values.First());
        }

        [Fact]
        public void ShouldSetWhereNextXFiscalYears()
        {
            var query = new Query<Account>(null)
                .WhereNextXFiscalYears(Account.Fields.CreatedOn, 10);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.NextXFiscalYears, query.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal(10, query.QueryExpression.Criteria.Conditions.First().Values.First());

            var query2 = new Query<Account>(null)
                .WhereNextXFiscalYears(Account.Fields.CreatedOn, 10, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.NextXFiscalYears, query2.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal(10, query2.QueryExpression.Criteria.Conditions.First().Values.First());
        }

        [Fact]
        public void ShouldSetWhereNextXHours()
        {
            var query = new Query<Account>(null)
                .WhereNextXHours(Account.Fields.CreatedOn, 10);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.NextXHours, query.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal(10, query.QueryExpression.Criteria.Conditions.First().Values.First());

            var query2 = new Query<Account>(null)
                .WhereNextXHours(Account.Fields.CreatedOn, 10, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.NextXHours, query2.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal(10, query2.QueryExpression.Criteria.Conditions.First().Values.First());
        }

        [Fact]
        public void ShouldSetWhereNextXMonths()
        {
            var query = new Query<Account>(null)
                .WhereNextXMonths(Account.Fields.CreatedOn, 10);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.NextXMonths, query.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal(10, query.QueryExpression.Criteria.Conditions.First().Values.First());

            var query2 = new Query<Account>(null)
                .WhereNextXMonths(Account.Fields.CreatedOn, 10, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.NextXMonths, query2.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal(10, query2.QueryExpression.Criteria.Conditions.First().Values.First());
        }

        [Fact]
        public void ShouldSetWhereNextXWeeks()
        {
            var query = new Query<Account>(null)
                .WhereNextXWeeks(Account.Fields.CreatedOn, 10);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.NextXWeeks, query.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal(10, query.QueryExpression.Criteria.Conditions.First().Values.First());

            var query2 = new Query<Account>(null)
                .WhereNextXWeeks(Account.Fields.CreatedOn, 10, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.NextXWeeks, query2.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal(10, query2.QueryExpression.Criteria.Conditions.First().Values.First());
        }

        [Fact]
        public void ShouldSetWhereNextXYears()
        {
            var query = new Query<Account>(null)
                .WhereNextXYears(Account.Fields.CreatedOn, 10);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.NextXYears, query.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal(10, query.QueryExpression.Criteria.Conditions.First().Values.First());

            var query2 = new Query<Account>(null)
                .WhereNextXYears(Account.Fields.CreatedOn, 10, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.NextXYears, query2.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal(10, query2.QueryExpression.Criteria.Conditions.First().Values.First());
        }

        [Fact]
        public void ShouldSetWhereNextYear()
        {
            var query = new Query<Account>(null)
                .WhereNextYear(Account.Fields.CreatedOn);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.NextYear, query.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Empty(query.QueryExpression.Criteria.Conditions.First().Values);

            var query2 = new Query<Account>(null)
                .WhereNextYear(Account.Fields.CreatedOn, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.NextYear, query2.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Empty(query2.QueryExpression.Criteria.Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereNotBetween()
        {
            var query = new Query<Account>(null)
                .WhereNotBetween(Account.Fields.NumberOfEmployees, 10, 50);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.NumberOfEmployees);
            Assert.Equal(ConditionOperator.NotBetween, query.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal(10, query.QueryExpression.Criteria.Conditions.First().Values[0]);
            Assert.Equal(50, query.QueryExpression.Criteria.Conditions.First().Values[1]);

            var query2 = new Query<Account>(null)
                .WhereNotBetween(
                    Account.Fields.NumberOfEmployees,
                    10,
                    50,
                    Account.EntityLogicalName
                );

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.NumberOfEmployees);
            Assert.Equal(ConditionOperator.NotBetween, query2.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal(10, query2.QueryExpression.Criteria.Conditions.First().Values[0]);
            Assert.Equal(50, query2.QueryExpression.Criteria.Conditions.First().Values[1]);
        }

        [Fact]
        public void ShouldSetWhereNotEqual()
        {
            var query = new Query<Account>(null)
                .WhereNotEqual(Account.Fields.Name, "test");

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(ConditionOperator.NotEqual, query.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal("test", query.QueryExpression.Criteria.Conditions.First().Values.First());

            var query2 = new Query<Account>(null)
                .WhereNotEqual(Account.Fields.Name, "test", Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(ConditionOperator.NotEqual, query2.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal("test", query2.QueryExpression.Criteria.Conditions.First().Values.First());
        }

        [Fact]
        public void ShouldSetWhereNotEqualBusinessId()
        {
            var query = new Query<Account>(null)
                .WhereNotEqualBusinessId(Account.Fields.OwningBusinessUnit);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.OwningBusinessUnit);
            Assert.Equal(ConditionOperator.NotEqualBusinessId, query.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Empty(query.QueryExpression.Criteria.Conditions.First().Values);

            var query2 = new Query<Account>(null)
                .WhereNotEqualBusinessId(Account.Fields.OwningBusinessUnit, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.OwningBusinessUnit);
            Assert.Equal(ConditionOperator.NotEqualBusinessId, query2.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Empty(query2.QueryExpression.Criteria.Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereNotEqualUserId()
        {
            var query = new Query<Account>(null)
                .WhereNotEqualUserId(Account.Fields.OwnerId);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.OwnerId);
            Assert.Equal(ConditionOperator.NotEqualUserId, query.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Empty(query.QueryExpression.Criteria.Conditions.First().Values);

            var query2 = new Query<Account>(null)
                .WhereNotEqualUserId(Account.Fields.OwnerId, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.OwnerId);
            Assert.Equal(ConditionOperator.NotEqualUserId, query2.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Empty(query2.QueryExpression.Criteria.Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereNotIn()
        {
            var query = new Query<Account>(null)
                .WhereNotIn(
                    Account.Fields.CustomerTypeCode,
                    1,
                    2,
                    3
                );

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CustomerTypeCode);
            Assert.Equal(ConditionOperator.NotIn, query.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Contains(1, query.QueryExpression.Criteria.Conditions.First().Values);
            Assert.Contains(2, query.QueryExpression.Criteria.Conditions.First().Values);
            Assert.Contains(3, query.QueryExpression.Criteria.Conditions.First().Values);

            var query2 = new Query<Account>(null)
                .WhereNotIn(
                    Account.EntityLogicalName,
                    Account.Fields.CustomerTypeCode,
                    1,
                    2,
                    3
                );

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CustomerTypeCode);
            Assert.Equal(ConditionOperator.NotIn, query2.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Contains(1, query2.QueryExpression.Criteria.Conditions.First().Values);
            Assert.Contains(2, query2.QueryExpression.Criteria.Conditions.First().Values);
            Assert.Contains(3, query2.QueryExpression.Criteria.Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereNotInList()
        {
            var query = new Query<Account>(null)
                .WhereNotIn(
                    Account.Fields.CustomerTypeCode,
                    new List<int>
                    {
                        1,
                        2,
                        3
                    }
                );

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CustomerTypeCode);
            Assert.Equal(ConditionOperator.NotIn, query.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.IsAssignableFrom<IList>(query.QueryExpression.Criteria.Conditions.First().Values);

            var query2 = new Query<Account>(null)
                .WhereNotIn(
                    Account.EntityLogicalName,
                    Account.Fields.CustomerTypeCode,
                    new List<int>
                    {
                        1,
                        2,
                        3
                    }
                );

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CustomerTypeCode);
            Assert.Equal(ConditionOperator.NotIn, query2.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.IsAssignableFrom<IList>(query.QueryExpression.Criteria.Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereNotLike()
        {
            var query = new Query<Account>(null)
                .WhereNotLike(Account.Fields.Name, "%test%");

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(ConditionOperator.NotLike, query.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal("%test%", query.QueryExpression.Criteria.Conditions.First().Values.First());

            var query2 = new Query<Account>(null)
                .WhereNotLike(Account.Fields.Name, "%test%", Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(ConditionOperator.NotLike, query2.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal("%test%", query2.QueryExpression.Criteria.Conditions.First().Values.First());
        }

        [Fact]
        public void ShouldSetWhereNotMask()
        {
            var obj = new object();
            var query = new Query<Account>(null)
                .WhereNotMask(Account.Fields.Name, obj);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(ConditionOperator.NotMask, query.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.First(), obj);

            var query2 = new Query<Account>(null)
                .WhereNotMask(Account.Fields.Name, obj, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(ConditionOperator.NotMask, query2.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.First(), obj);
        }

        [Fact]
        public void ShouldSetWhereNotNull()
        {
            var query = new Query<Account>(null)
                .WhereNotNull(Account.Fields.Name);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(ConditionOperator.NotNull, query.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Empty(query.QueryExpression.Criteria.Conditions.First().Values);

            var query2 = new Query<Account>(null)
                .WhereNotNull(Account.Fields.Name, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(ConditionOperator.NotNull, query2.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Empty(query2.QueryExpression.Criteria.Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereNotOn()
        {
            var date = new DateTime();
            var query = new Query<Account>(null)
                .WhereNotOn(Account.Fields.CreatedOn, date);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.NotOn, query.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.First(), date);

            var query2 = new Query<Account>(null)
                .WhereNotOn(Account.Fields.CreatedOn, date, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.NotOn, query2.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.First(), date);
        }

        [Fact]
        public void ShouldSetWhereNotUnder()
        {
            var guid = new Guid();
            var query = new Query<Account>(null)
                .WhereNotUnder(Account.Fields.AccountId, guid);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.AccountId);
            Assert.Equal(ConditionOperator.NotUnder, query.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.First(), guid);

            var query2 = new Query<Account>(null)
                .WhereNotUnder(Account.Fields.AccountId, guid, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.AccountId);
            Assert.Equal(ConditionOperator.NotUnder, query2.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.First(), guid);
        }

        [Fact]
        public void ShouldSetWhereNull()
        {
            var query = new Query<Account>(null)
                .WhereNull(Account.Fields.Name);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(ConditionOperator.Null, query.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Empty(query.QueryExpression.Criteria.Conditions.First().Values);

            var query2 = new Query<Account>(null)
                .WhereNull(Account.Fields.Name, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(ConditionOperator.Null, query2.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Empty(query2.QueryExpression.Criteria.Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereOlderThanXDays()
        {
            var query = new Query<Account>(null)
                .WhereOlderThanXDays(Account.Fields.CreatedOn, 10);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.OlderThanXDays, query.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal(10, query.QueryExpression.Criteria.Conditions.First().Values.First());

            var query2 = new Query<Account>(null)
                .WhereOlderThanXDays(Account.Fields.CreatedOn, 10, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.OlderThanXDays, query2.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal(10, query2.QueryExpression.Criteria.Conditions.First().Values.First());
        }

        [Fact]
        public void ShouldSetWhereOlderThanXHours()
        {
            var query = new Query<Account>(null)
                .WhereOlderThanXHours(Account.Fields.CreatedOn, 10);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.OlderThanXHours, query.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal(10, query.QueryExpression.Criteria.Conditions.First().Values.First());

            var query2 = new Query<Account>(null)
                .WhereOlderThanXHours(Account.Fields.CreatedOn, 10, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.OlderThanXHours, query2.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal(10, query2.QueryExpression.Criteria.Conditions.First().Values.First());
        }

        [Fact]
        public void ShouldSetWhereOlderThanXMinutes()
        {
            var query = new Query<Account>(null)
                .WhereOlderThanXMinutes(Account.Fields.CreatedOn, 10);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.OlderThanXMinutes, query.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal(10, query.QueryExpression.Criteria.Conditions.First().Values.First());

            var query2 = new Query<Account>(null)
                .WhereOlderThanXMinutes(Account.Fields.CreatedOn, 10, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.OlderThanXMinutes, query2.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal(10, query2.QueryExpression.Criteria.Conditions.First().Values.First());
        }

        [Fact]
        public void ShouldSetWhereOlderThanXMonths()
        {
            var query = new Query<Account>(null)
                .WhereOlderThanXMonths(Account.Fields.CreatedOn, 10);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.OlderThanXMonths, query.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal(10, query.QueryExpression.Criteria.Conditions.First().Values.First());

            var query2 = new Query<Account>(null)
                .WhereOlderThanXMonths(Account.Fields.CreatedOn, 10, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.OlderThanXMonths, query2.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal(10, query2.QueryExpression.Criteria.Conditions.First().Values.First());
        }

        [Fact]
        public void ShouldSetWhereOlderThanXWeeks()
        {
            var query = new Query<Account>(null)
                .WhereOlderThanXWeeks(Account.Fields.CreatedOn, 10);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.OlderThanXWeeks, query.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal(10, query.QueryExpression.Criteria.Conditions.First().Values.First());

            var query2 = new Query<Account>(null)
                .WhereOlderThanXWeeks(Account.Fields.CreatedOn, 10, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.OlderThanXWeeks, query2.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal(10, query2.QueryExpression.Criteria.Conditions.First().Values.First());
        }

        [Fact]
        public void ShouldSetWhereOlderThanXYears()
        {
            var query = new Query<Account>(null)
                .WhereOlderThanXYears(Account.Fields.CreatedOn, 10);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.OlderThanXYears, query.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal(10, query.QueryExpression.Criteria.Conditions.First().Values.First());

            var query2 = new Query<Account>(null)
                .WhereOlderThanXYears(Account.Fields.CreatedOn, 10, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.OlderThanXYears, query2.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal(10, query2.QueryExpression.Criteria.Conditions.First().Values.First());
        }

        [Fact]
        public void ShouldSetWhereOn()
        {
            var date = new DateTime();
            var query = new Query<Account>(null)
                .WhereOn(Account.Fields.CreatedOn, date);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.On, query.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.First(), date);

            var query2 = new Query<Account>(null)
                .WhereOn(Account.Fields.CreatedOn, date, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.On, query2.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.First(), date);
        }

        [Fact]
        public void ShouldSetWhereOnOrAfter()
        {
            var date = new DateTime();
            var query = new Query<Account>(null)
                .WhereOnOrAfter(Account.Fields.CreatedOn, date);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.OnOrAfter, query.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.First(), date);

            var query2 = new Query<Account>(null)
                .WhereOnOrAfter(Account.Fields.CreatedOn, date, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.OnOrAfter, query2.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.First(), date);
        }

        [Fact]
        public void ShouldSetWhereOnOrBefore()
        {
            var date = new DateTime();
            var query = new Query<Account>(null)
                .WhereOnOrBefore(Account.Fields.CreatedOn, date);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.OnOrBefore, query.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.First(), date);

            var query2 = new Query<Account>(null)
                .WhereOnOrBefore(Account.Fields.CreatedOn, date, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.OnOrBefore, query2.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.First(), date);
        }

        [Fact]
        public void ShouldSetWhereThisFiscalPeriod()
        {
            var query = new Query<Account>(null)
                .WhereThisFiscalPeriod(Account.Fields.CreatedOn);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.ThisFiscalPeriod, query.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Empty(query.QueryExpression.Criteria.Conditions.First().Values);

            var query2 = new Query<Account>(null)
                .WhereThisFiscalPeriod(Account.Fields.CreatedOn, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.ThisFiscalPeriod, query2.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Empty(query2.QueryExpression.Criteria.Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereThisFiscalYear()
        {
            var query = new Query<Account>(null)
                .WhereThisFiscalYear(Account.Fields.CreatedOn);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.ThisFiscalYear, query.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Empty(query.QueryExpression.Criteria.Conditions.First().Values);

            var query2 = new Query<Account>(null)
                .WhereThisFiscalYear(Account.Fields.CreatedOn, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.ThisFiscalYear, query2.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Empty(query2.QueryExpression.Criteria.Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereThisMonth()
        {
            var query = new Query<Account>(null)
                .WhereThisMonth(Account.Fields.CreatedOn);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.ThisMonth, query.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Empty(query.QueryExpression.Criteria.Conditions.First().Values);

            var query2 = new Query<Account>(null)
                .WhereThisMonth(Account.Fields.CreatedOn, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.ThisMonth, query2.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Empty(query2.QueryExpression.Criteria.Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereThisWeek()
        {
            var query = new Query<Account>(null)
                .WhereThisWeek(Account.Fields.CreatedOn);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.ThisWeek, query.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Empty(query.QueryExpression.Criteria.Conditions.First().Values);

            var query2 = new Query<Account>(null)
                .WhereThisWeek(Account.Fields.CreatedOn, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.ThisWeek, query2.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Empty(query2.QueryExpression.Criteria.Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereThisYear()
        {
            var query = new Query<Account>(null)
                .WhereThisYear(Account.Fields.CreatedOn);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.ThisYear, query.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Empty(query.QueryExpression.Criteria.Conditions.First().Values);

            var query2 = new Query<Account>(null)
                .WhereThisYear(Account.Fields.CreatedOn, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.ThisYear, query2.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Empty(query2.QueryExpression.Criteria.Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereToday()
        {
            var query = new Query<Account>(null)
                .WhereToday(Account.Fields.CreatedOn);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.Today, query.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Empty(query.QueryExpression.Criteria.Conditions.First().Values);

            var query2 = new Query<Account>(null)
                .WhereToday(Account.Fields.CreatedOn, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.Today, query2.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Empty(query2.QueryExpression.Criteria.Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereTomorrow()
        {
            var query = new Query<Account>(null)
                .WhereTomorrow(Account.Fields.CreatedOn);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.Tomorrow, query.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Empty(query.QueryExpression.Criteria.Conditions.First().Values);

            var query2 = new Query<Account>(null)
                .WhereTomorrow(Account.Fields.CreatedOn, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.Tomorrow, query2.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Empty(query2.QueryExpression.Criteria.Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereUnder()
        {
            var guid = new Guid();
            var query = new Query<Account>(null)
                .WhereUnder(Account.Fields.AccountId, guid);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.AccountId);
            Assert.Equal(ConditionOperator.Under, query.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.First(), guid);

            var query2 = new Query<Account>(null)
                .WhereUnder(Account.Fields.AccountId, guid, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.AccountId);
            Assert.Equal(ConditionOperator.Under, query2.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.First(), guid);
        }

        [Fact]
        public void ShouldSetWhereUnderOrEqual()
        {
            var guid = new Guid();
            var query = new Query<Account>(null)
                .WhereUnderOrEqual(Account.Fields.AccountId, guid);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.AccountId);
            Assert.Equal(ConditionOperator.UnderOrEqual, query.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.First(), guid);

            var query2 = new Query<Account>(null)
                .WhereUnderOrEqual(Account.Fields.AccountId, guid, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.AccountId);
            Assert.Equal(ConditionOperator.UnderOrEqual, query2.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.First(), guid);
        }

        [Fact]
        public void ShouldSetWhereYesterday()
        {
            var query = new Query<Account>(null)
                .WhereYesterday(Account.Fields.CreatedOn);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.Yesterday, query.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Empty(query.QueryExpression.Criteria.Conditions.First().Values);

            var query2 = new Query<Account>(null)
                .WhereYesterday(Account.Fields.CreatedOn, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.Yesterday, query2.QueryExpression.Criteria.Conditions.First().Operator);
            Assert.Empty(query2.QueryExpression.Criteria.Conditions.First().Values);
        }

        #endregion Conditions

        #region IOrganizationService calls

        private readonly Guid _item1Id = Guid.NewGuid();
        private readonly Guid _item2Id = Guid.NewGuid();

        [Fact]
        public void ShouldGetAll()
        {
            var fakedContext = new XrmFakedContext { ProxyTypesAssembly = typeof(Account).Assembly };
            fakedContext.Initialize(new Account { Id = _item1Id });
            var service = fakedContext.GetOrganizationService();

            var records = new Query<Account>(service)
                .GetAll();

            records.Should().HaveCount(1);
            records.First().Id.Should().Be(_item1Id);
        }

        [Fact]
        public void ShouldGetAllTopCount()
        {
            var fakedContext = new XrmFakedContext { ProxyTypesAssembly = typeof(Account).Assembly };
            fakedContext.Initialize(new Account { Id = _item1Id });
            var service = fakedContext.GetOrganizationService();

            var records = new Query<Account>(service)
                          .Top(100)
                          .GetAll();

            Assert.Single(records);
            Assert.Equal(records.First().Id, _item1Id);
        }

        [Fact]
        public void ShouldQueryWithExtension()
        {
            var fakedContext = new XrmFakedContext { ProxyTypesAssembly = typeof(Account).Assembly };
            fakedContext.Initialize(new Account { Id = _item1Id });
            var service = fakedContext.GetOrganizationService();

            var records = service.Query<Account>().GetAll();

            Assert.Single(records);
            Assert.Equal(records.First().Id, _item1Id);
        }

        [Fact]
        public void ShouldGetAllWithoutPaging()
        {
            var fakedContext = new XrmFakedContext { ProxyTypesAssembly = typeof(Account).Assembly };
            fakedContext.Initialize(new Account { Id = _item1Id });
            var service = fakedContext.GetOrganizationService();

            var records = new Query<Account>(service).GetAll();

            Assert.Single(records);
            Assert.Equal(records.First().Id, _item1Id);
        }

        [Fact]
        public void ShouldGetById()
        {
            var fakedContext = new XrmFakedContext { ProxyTypesAssembly = typeof(Account).Assembly };
            fakedContext.Initialize(new Account { Id = _item1Id });
            var service = fakedContext.GetOrganizationService();

            var query = new Query<Account>(service);
            var record = query.GetById(_item1Id);

            Assert.NotNull(record);
            Assert.Equal($"{Account.EntityLogicalName}id", query.QueryExpression.Criteria.Conditions.First().AttributeName);
        }

        [Fact]
        public void ShouldGetByIdForActivity()
        {
            var fakedContext = new XrmFakedContext { ProxyTypesAssembly = typeof(Account).Assembly };
            fakedContext.Initialize(new Task { Id = _item1Id });

            var service = fakedContext.GetOrganizationService();

            var query = new Query<Task>(service);
            var record = query.GetById(_item1Id, true);

            Assert.NotNull(record);
            Assert.Equal("activityid", query.QueryExpression.Criteria.Conditions.First().AttributeName);
        }

        [Fact]
        public void ShouldGetFirst()
        {
            var fakedContext = new XrmFakedContext { ProxyTypesAssembly = typeof(Account).Assembly };
            fakedContext.Initialize(new List<Entity> { new Account { Id = _item1Id }, new Account { Id = _item2Id } });
            var service = fakedContext.GetOrganizationService();

            var record = new Query<Account>(service).GetFirst();

            Assert.NotNull(record);
            Assert.Equal(record.Id, _item1Id);
        }

        [Fact]
        public void ShouldGetFirstNull()
        {
            var fakedContext = new XrmFakedContext { ProxyTypesAssembly = typeof(Account).Assembly };
            var service = fakedContext.GetOrganizationService();

            var ex = Record.Exception(() => new Query<Account>(service).GetFirst());

            Assert.IsType<InvalidOperationException>(ex);
        }

        [Fact]
        public void ShouldGetFirstOrDefault()
        {
            var fakedContext = new XrmFakedContext { ProxyTypesAssembly = typeof(Account).Assembly };
            fakedContext.Initialize(new List<Entity> { new Account { Id = _item1Id }, new Account { Id = _item2Id } });
            var service = fakedContext.GetOrganizationService();

            var record = new Query<Account>(service).GetFirstOrDefault();

            Assert.NotNull(record);
            Assert.Equal(record.Id, _item1Id);
        }

        [Fact]
        public void ShouldGetFirstOrDefaultIsNull()
        {
            var fakedContext = new XrmFakedContext { ProxyTypesAssembly = typeof(Account).Assembly };
            var service = fakedContext.GetOrganizationService();

            var record = new Query<Account>(service).GetFirstOrDefault();

            Assert.Null(record);
        }

        [Fact]
        public void ShouldGetLast()
        {
            var fakedContext = new XrmFakedContext { ProxyTypesAssembly = typeof(Account).Assembly };
            fakedContext.Initialize(new List<Entity> { new Account { Id = _item1Id }, new Account { Id = _item2Id } });
            var service = fakedContext.GetOrganizationService();

            var record = new Query<Account>(service).GetLast();

            Assert.NotNull(record);
            Assert.Equal(record.Id, _item2Id);
        }

        [Fact]
        public void ShouldGetLastOrDefault()
        {
            var fakedContext = new XrmFakedContext { ProxyTypesAssembly = typeof(Account).Assembly };
            fakedContext.Initialize(new List<Entity> { new Account { Id = _item1Id }, new Account { Id = _item2Id } });
            var service = fakedContext.GetOrganizationService();

            var record = new Query<Account>(service).GetLastOrDefault();

            Assert.NotNull(record);
            Assert.Equal(record.Id, _item2Id);
        }

        [Fact]
        public void ShouldGetLastOrDefaultIsNull()
        {
            var fakedContext = new XrmFakedContext { ProxyTypesAssembly = typeof(Account).Assembly };
            var service = fakedContext.GetOrganizationService();

            var record = new Query<Account>(service).GetLastOrDefault();

            Assert.Null(record);
        }

        [Fact]
        public void ShouldGetResults()
        {
            var fakedContext = new XrmFakedContext { ProxyTypesAssembly = typeof(Account).Assembly };
            fakedContext.Initialize(new List<Entity> { new Account { Id = _item1Id }, new Account { Id = _item2Id } });
            var service = fakedContext.GetOrganizationService();

            var records = new Query<Account>(service).GetResults();

            Assert.Equal(2, records.Entities.Count);
        }

        [Fact]
        public void ShouldGetSingle()
        {
            var fakedContext = new XrmFakedContext { ProxyTypesAssembly = typeof(Account).Assembly };
            fakedContext.Initialize(new List<Entity> { new Account { Id = _item1Id } });
            var service = fakedContext.GetOrganizationService();

            var record = new Query<Account>(service).GetSingle();

            Assert.NotNull(record);
            Assert.Equal(record.Id, _item1Id);
        }

        [Fact]
        public void ShouldGetSingleMany()
        {
            var fakedContext = new XrmFakedContext { ProxyTypesAssembly = typeof(Account).Assembly };
            fakedContext.Initialize(new List<Entity> { new Account { Id = _item1Id }, new Account { Id = _item2Id } });
            var service = fakedContext.GetOrganizationService();

            var ex = Record.Exception(() => new Query<Account>(service).GetSingle());

            Assert.IsType<InvalidOperationException>(ex);
        }

        [Fact]
        public void ShouldGetSingleNull()
        {
            var fakedContext = new XrmFakedContext { ProxyTypesAssembly = typeof(Account).Assembly };
            var service = fakedContext.GetOrganizationService();

            var ex = Record.Exception(() => new Query<Account>(service).GetSingle());

            Assert.IsType<InvalidOperationException>(ex);
        }

        [Fact]
        public void ShouldGetSingleOrDefault()
        {
            var fakedContext = new XrmFakedContext { ProxyTypesAssembly = typeof(Account).Assembly };
            fakedContext.Initialize(new List<Entity> { new Account { Id = _item1Id } });
            var service = fakedContext.GetOrganizationService();

            var record = new Query<Account>(service).GetSingleOrDefault();

            Assert.NotNull(record);
            Assert.Equal(record.Id, _item1Id);
        }

        [Fact]
        public void ShouldGetSingleOrDefaultIsNull()
        {
            var fakedContext = new XrmFakedContext { ProxyTypesAssembly = typeof(Account).Assembly };
            var service = fakedContext.GetOrganizationService();

            var record = new Query<Account>(service).GetSingleOrDefault();

            Assert.Null(record);
        }

        #endregion IOrganizationService calls
    }
}
