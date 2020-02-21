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
            var query = new Query<Account>()
                .Select(true);

            query.QueryExpression.ColumnSet.AllColumns
                 .Should()
                 .BeTrue();
        }

        [Fact]
        public void ShouldAddAttributesWithAnonymousType()
        {
            var query = new Query<Account>()
                .Select(a => new { a.Name, a.AccountNumber });

            Assert.Contains(Account.Fields.Name, query.QueryExpression.ColumnSet.Columns);
            Assert.Contains(Account.Fields.AccountNumber, query.QueryExpression.ColumnSet.Columns);
        }

        [Fact]
        public void ShouldAddFilter()
        {
            var query = new Query<Account>()
                .AddFilters(LogicalOperator.Or, new Filter(LogicalOperator.Or));

            Assert.Equal(LogicalOperator.Or, query.QueryExpression.Criteria.FilterOperator);
            Assert.Single(query.QueryExpression.Criteria.Filters);
            Assert.Equal(query.QueryExpression.Criteria.Filters.First().FilterOperator, LogicalOperator.Or);
        }

        [Fact]
        public void ShouldAddFilterWithoutOperator()
        {
            var query = new Query<Account>()
                .AddFilters(new Filter(LogicalOperator.Or));

            Assert.Equal(query.QueryExpression.Criteria.FilterOperator, LogicalOperator.And);
            Assert.Equal(query.QueryExpression.Criteria.Filters.Count, 1);
            Assert.Equal(query.QueryExpression.Criteria.Filters.First().FilterOperator, LogicalOperator.Or);
        }

        [Fact]
        public void ShouldAddLink()
        {
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId));

            Assert.Equal(query.QueryExpression.LinkEntities.Count, 1);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkToEntityName, Contact.EntityLogicalName);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkToAttributeName, Contact.Fields.ParentCustomerId);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkFromAttributeName, Account.Fields.AccountId);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkFromEntityName, Account.EntityLogicalName);
            Assert.Equal(query.QueryExpression.LinkEntities.First().EntityAlias, Contact.EntityLogicalName);
        }

        [Fact]
        public void ShouldAddLinks()
        {
            var query = new Query<Account>()
                        .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId))
                        .AddLink(new Link<Task>(Task.Fields.RegardingObjectId, Account.Fields.AccountId));

            Assert.Equal(query.QueryExpression.LinkEntities.Count, 2);
            Assert.Equal(query.QueryExpression.LinkEntities[0].LinkToEntityName, Contact.EntityLogicalName);
            Assert.Equal(query.QueryExpression.LinkEntities[0].LinkToAttributeName, Contact.Fields.ParentCustomerId);
            Assert.Equal(query.QueryExpression.LinkEntities[0].LinkFromAttributeName, Account.Fields.AccountId);
            Assert.Equal(query.QueryExpression.LinkEntities[0].LinkFromEntityName, Account.EntityLogicalName);
            Assert.Equal(query.QueryExpression.LinkEntities[0].EntityAlias, Contact.EntityLogicalName);
            Assert.Equal(query.QueryExpression.LinkEntities[1].LinkToEntityName, Task.EntityLogicalName);
            Assert.Equal(query.QueryExpression.LinkEntities[1].LinkToAttributeName, Task.Fields.RegardingObjectId);
            Assert.Equal(query.QueryExpression.LinkEntities[1].LinkFromAttributeName, Account.Fields.AccountId);
            Assert.Equal(query.QueryExpression.LinkEntities[1].LinkFromEntityName, Account.EntityLogicalName);
            Assert.Equal(query.QueryExpression.LinkEntities[1].EntityAlias, Task.EntityLogicalName);
        }

        [Fact]
        public void ShouldAddNoAttribute()
        {
            var query = new Query<Account>()
                .Select();

            Assert.Equal(query.QueryExpression.ColumnSet.AllColumns, false);
        }

        [Fact]
        public void ShouldAddOneAttribute()
        {
            var query = new Query<Account>()
                .Select(Account.Fields.Name);

            Assert.True(query.QueryExpression.ColumnSet.Columns.Contains(Account.Fields.Name));
        }

        [Fact]
        public void ShouldAddOrder()
        {
            var query = new Query<Account>()
                .Order(Account.Fields.Name, OrderType.Ascending);

            Assert.Equal(query.QueryExpression.Orders.First().AttributeName, Account.Fields.Name);
            Assert.Equal(query.QueryExpression.Orders.First().OrderType, OrderType.Ascending);
        }

        [Fact]
        public void ShouldAddPaging()
        {
            var query = new Query<Account>()
                .SetPagingInfo(1, 100, true);

            Assert.Equal(query.QueryExpression.PageInfo.PageNumber, 1);
            Assert.Equal(query.QueryExpression.PageInfo.Count, 100);
            Assert.Equal(query.QueryExpression.PageInfo.ReturnTotalRecordCount, true);
        }

        [Fact]
        public void ShouldAddTwoAttributes()
        {
            var query = new Query<Account>()
                        .Select(Account.Fields.Name)
                        .Select(Account.Fields.AccountNumber);

            Assert.True(query.QueryExpression.ColumnSet.Columns.Contains(Account.Fields.Name));
            Assert.True(query.QueryExpression.ColumnSet.Columns.Contains(Account.Fields.AccountNumber));
        }

        [Fact]
        public void ShouldAddTwoFilters()
        {
            var query = new Query<Account>()
                .AddFilters(
                    LogicalOperator.Or,
                    new Filter(LogicalOperator.Or),
                    new Filter()
                );

            Assert.Equal(query.QueryExpression.Criteria.FilterOperator, LogicalOperator.Or);
            Assert.Equal(query.QueryExpression.Criteria.Filters.Count, 2);
            Assert.Equal(query.QueryExpression.Criteria.Filters[0].FilterOperator, LogicalOperator.Or);
            Assert.Equal(query.QueryExpression.Criteria.Filters[1].FilterOperator, LogicalOperator.And);
        }

        [Fact]
        public void ShouldBeAccountQueryExpression()
        {
            var query = new Query<Account>();

            Assert.Equal(query.QueryExpression.EntityName, Account.EntityLogicalName);
        }

        [Fact]
        public void ShouldBeDistinct()
        {
            var query = new Query<Account>()
                .Distinct();

            Assert.Equal(query.QueryExpression.Distinct, true);
        }

        [Fact]
        public void ShouldCreateLateBound()
        {
            var query = new Query(Account.EntityLogicalName);

            Assert.Equal(query.QueryExpression.EntityName, Account.EntityLogicalName);
        }

        [Fact]
        public void ShouldHaveNoLock()
        {
            var query = new Query<Account>()
                .NoLock();

            Assert.Equal(query.QueryExpression.NoLock, true);
        }

        [Fact]
        public void ShouldSetLogicalOperatorOr()
        {
            var query = new Query<Account>()
                .SetDefaultFilterOperator(LogicalOperator.Or);

            Assert.Equal(query.QueryExpression.Criteria.FilterOperator, LogicalOperator.Or);
        }

        [Fact]
        public void ShouldSetNextPage()
        {
            var query = new Query<Account>()
                        .SetPagingInfo(1, 100, true)
                        .NextPage("<fakePagingCookie>");

            Assert.Equal(query.QueryExpression.PageInfo.PageNumber, 2);
            Assert.Equal(query.QueryExpression.PageInfo.Count, 100);
            Assert.Equal(query.QueryExpression.PageInfo.ReturnTotalRecordCount, true);
            Assert.Equal(query.QueryExpression.PageInfo.PagingCookie, "<fakePagingCookie>");
        }

        [Fact]
        public void ShouldSetTop()
        {
            var query = new Query<Account>()
                .Top(100);

            Assert.Equal(query.QueryExpression.TopCount, 100);
        }

        #region Conditions

        [Fact]
        public void ShouldSetWhere()
        {
            var guid = Guid.NewGuid();
            var query = new Query<Account>()
                .Where(Account.Fields.AccountId, ConditionOperator.Above, guid);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.AccountId);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.Above);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.Single(), guid);

            var query2 = new Query<Account>()
                .Where(
                    Account.EntityLogicalName,
                    Account.Fields.AccountId,
                    ConditionOperator.Above,
                    guid
                );

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.AccountId);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.Above);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.Single(), guid);
        }

        [Fact]
        public void ShouldSetWhereAbove()
        {
            var guid = Guid.NewGuid();
            var query = new Query<Account>()
                .WhereAbove(Account.Fields.AccountId, guid);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.AccountId);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.Above);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.Single(), guid);

            var query2 = new Query<Account>()
                .WhereAbove(Account.Fields.AccountId, guid, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.AccountId);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.Above);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.Single(), guid);
        }

        [Fact]
        public void ShouldSetWhereAboveOrEqual()
        {
            var guid = Guid.NewGuid();
            var query = new Query<Account>()
                .WhereAboveOrEqual(Account.Fields.AccountId, guid);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.AccountId);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.AboveOrEqual);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.Single(), guid);

            var query2 = new Query<Account>()
                .WhereAboveOrEqual(Account.Fields.AccountId, guid, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.AccountId);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.AboveOrEqual);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.Single(), guid);
        }

        [Fact]
        public void ShouldSetWhereBeginsWith()
        {
            var query = new Query<Account>()
                .WhereBeginsWith(Account.Fields.Name, "test");

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.BeginsWith);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.Single(), "test");

            var query2 = new Query<Account>()
                .WhereBeginsWith(Account.Fields.Name, "test", Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.BeginsWith);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.Single(), "test");
        }

        [Fact]
        public void ShouldSetWhereBetween()
        {
            var query = new Query<Account>()
                .WhereBetween(Account.Fields.NumberOfEmployees, 10, 50);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.NumberOfEmployees);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.Between);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values[0], 10);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values[1], 50);

            var query2 = new Query<Account>()
                .WhereBetween(
                    Account.Fields.NumberOfEmployees,
                    10,
                    50,
                    Account.EntityLogicalName
                );

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.NumberOfEmployees);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.Between);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values[0], 10);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values[1], 50);
        }

        [Fact]
        public void ShouldSetWhereChildOf()
        {
            var guid = Guid.NewGuid();
            var query = new Query<Account>()
                .WhereChildOf(Account.Fields.AccountId, guid);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.AccountId);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.ChildOf);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.Single(), guid);

            var query2 = new Query<Account>()
                .WhereChildOf(Account.Fields.AccountId, guid, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.AccountId);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.ChildOf);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.Single(), guid);
        }

        [Fact]
        public void ShouldSetWhereContains()
        {
            var query = new Query<Account>()
                .WhereContains(Account.Fields.Name, "test");

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.Contains);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.Single(), "test");

            var query2 = new Query<Account>()
                .WhereContains(Account.Fields.Name, "test", Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.Contains);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.Single(), "test");
        }

        [Fact]
        public void ShouldSetWhereContainValues()
        {
            var query = new Query<Account>()
                .WhereContainValues(
                    Account.Fields.CustomerTypeCode,
                    1,
                    2,
                    3
                );

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CustomerTypeCode);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.ContainValues);
            Assert.True(query.QueryExpression.Criteria.Conditions.First().Values.Contains(1));
            Assert.True(query.QueryExpression.Criteria.Conditions.First().Values.Contains(2));
            Assert.True(query.QueryExpression.Criteria.Conditions.First().Values.Contains(3));

            var query2 = new Query<Account>()
                .WhereContainValues(
                    Account.EntityLogicalName,
                    Account.Fields.CustomerTypeCode,
                    1,
                    2,
                    3
                );

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CustomerTypeCode);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.ContainValues);
            Assert.True(query2.QueryExpression.Criteria.Conditions.First().Values.Contains(1));
            Assert.True(query2.QueryExpression.Criteria.Conditions.First().Values.Contains(2));
            Assert.True(query2.QueryExpression.Criteria.Conditions.First().Values.Contains(3));
        }

        [Fact]
        public void ShouldSetWhereDoesNotBeginWith()
        {
            var query = new Query<Account>()
                .WhereDoesNotBeginWith(Account.Fields.Name, "test");

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.DoesNotBeginWith);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.Single(), "test");

            var query2 = new Query<Account>()
                .WhereDoesNotBeginWith(Account.Fields.Name, "test", Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.DoesNotBeginWith);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.Single(), "test");
        }

        [Fact]
        public void ShouldSetWhereDoesNotContain()
        {
            var query = new Query<Account>()
                .WhereDoesNotContain(Account.Fields.Name, "test");

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.DoesNotContain);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.Single(), "test");

            var query2 = new Query<Account>()
                .WhereDoesNotContain(Account.Fields.Name, "test", Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.DoesNotContain);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.Single(), "test");
        }

        [Fact]
        public void ShouldSetWhereDoesNotContainValues()
        {
            var query = new Query<Account>()
                .WhereDoesNotContainValues(
                    Account.Fields.CustomerTypeCode,
                    1,
                    2,
                    3
                );

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CustomerTypeCode);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.DoesNotContainValues);
            Assert.True(query.QueryExpression.Criteria.Conditions.First().Values.Contains(1));
            Assert.True(query.QueryExpression.Criteria.Conditions.First().Values.Contains(2));
            Assert.True(query.QueryExpression.Criteria.Conditions.First().Values.Contains(3));

            var query2 = new Query<Account>()
                .WhereDoesNotContainValues(
                    Account.EntityLogicalName,
                    Account.Fields.CustomerTypeCode,
                    1,
                    2,
                    3
                );

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CustomerTypeCode);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.DoesNotContainValues);
            Assert.True(query2.QueryExpression.Criteria.Conditions.First().Values.Contains(1));
            Assert.True(query2.QueryExpression.Criteria.Conditions.First().Values.Contains(2));
            Assert.True(query2.QueryExpression.Criteria.Conditions.First().Values.Contains(3));
        }

        [Fact]
        public void ShouldSetWhereDoesNotEndWith()
        {
            var query = new Query<Account>()
                .WhereDoesNotEndWith(Account.Fields.Name, "test");

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.DoesNotEndWith);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.Single(), "test");

            var query2 = new Query<Account>()
                .WhereDoesNotEndWith(Account.Fields.Name, "test", Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.DoesNotEndWith);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.Single(), "test");
        }

        [Fact]
        public void ShouldSetWhereEndsWith()
        {
            var query = new Query<Account>()
                .WhereEndsWith(Account.Fields.Name, "test");

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.EndsWith);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.Single(), "test");

            var query2 = new Query<Account>()
                .WhereEndsWith(Account.Fields.Name, "test", Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.EndsWith);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.Single(), "test");
        }

        [Fact]
        public void ShouldSetWhereEqual()
        {
            var query = new Query<Account>()
                .WhereEqual(Account.Fields.Name, "test");

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.Equal);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.Single(), "test");

            var query2 = new Query<Account>()
                .WhereEqual(Account.Fields.Name, "test", Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.Equal);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.Single(), "test");
        }

        [Fact]
        public void ShouldSetWhereEqualBusinessId()
        {
            var query = new Query<Account>()
                .WhereEqualBusinessId(Account.Fields.OwningBusinessUnit);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.OwningBusinessUnit);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.EqualBusinessId);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.Count, 0);

            var query2 = new Query<Account>()
                .WhereEqualBusinessId(Account.Fields.OwningBusinessUnit, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.OwningBusinessUnit);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.EqualBusinessId);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.Count, 0);
        }

        [Fact]
        public void ShouldSetWhereEqualUserId()
        {
            var query = new Query<Account>()
                .WhereEqualUserId(Account.Fields.OwnerId);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.OwnerId);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.EqualUserId);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.Count, 0);

            var query2 = new Query<Account>()
                .WhereEqualUserId(Account.Fields.OwnerId, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.OwnerId);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.EqualUserId);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.Count, 0);
        }

        [Fact]
        public void ShouldSetWhereEqualUserLanguage()
        {
            var query = new Query<Account>()
                .WhereEqualUserLanguage("no_language_attribute");

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, "no_language_attribute");
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.EqualUserLanguage);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.Count, 0);

            var query2 = new Query<Account>()
                .WhereEqualUserLanguage("no_language_attribute", Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, "no_language_attribute");
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.EqualUserLanguage);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.Count, 0);
        }

        [Fact]
        public void ShouldSetWhereEqualUserOrUserHierarchy()
        {
            var query = new Query<Account>()
                .WhereEqualUserOrUserHierarchy(Account.Fields.OwnerId);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.OwnerId);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.EqualUserOrUserHierarchy);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.Count, 0);

            var query2 = new Query<Account>()
                .WhereEqualUserOrUserHierarchy(Account.Fields.OwnerId, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.OwnerId);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.EqualUserOrUserHierarchy);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.Count, 0);
        }

        [Fact]
        public void ShouldSetWhereEqualUserOrUserHierarchyAndTeams()
        {
            var query = new Query<Account>()
                .WhereEqualUserOrUserHierarchyAndTeams(Account.Fields.OwnerId);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.OwnerId);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.EqualUserOrUserHierarchyAndTeams);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.Count, 0);

            var query2 = new Query<Account>()
                .WhereEqualUserOrUserHierarchyAndTeams(Account.Fields.OwnerId, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.OwnerId);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.EqualUserOrUserHierarchyAndTeams);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.Count, 0);
        }

        [Fact]
        public void ShouldSetWhereEqualUserOrUserTeams()
        {
            var query = new Query<Account>()
                .WhereEqualUserOrUserTeams(Account.Fields.OwnerId);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.OwnerId);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.EqualUserOrUserTeams);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.Count, 0);

            var query2 = new Query<Account>()
                .WhereEqualUserOrUserTeams(Account.Fields.OwnerId, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.OwnerId);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.EqualUserOrUserTeams);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.Count, 0);
        }

        [Fact]
        public void ShouldSetWhereEqualUserTeams()
        {
            var query = new Query<Account>()
                .WhereEqualUserTeams(Account.Fields.OwnerId);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.OwnerId);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.EqualUserTeams);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.Count, 0);

            var query2 = new Query<Account>()
                .WhereEqualUserTeams(Account.Fields.OwnerId, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.OwnerId);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.EqualUserTeams);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.Count, 0);
        }

        [Fact]
        public void ShouldSetWhereGreaterEqual()
        {
            var query = new Query<Account>()
                .WhereGreaterEqual(Account.Fields.NumberOfEmployees, 10);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.NumberOfEmployees);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.GreaterEqual);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.First(), 10);

            var query2 = new Query<Account>()
                .WhereGreaterEqual(Account.Fields.NumberOfEmployees, 10, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.NumberOfEmployees);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.GreaterEqual);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.First(), 10);
        }

        [Fact]
        public void ShouldSetWhereGreaterThan()
        {
            var query = new Query<Account>()
                .WhereGreaterThan(Account.Fields.NumberOfEmployees, 10);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.NumberOfEmployees);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.GreaterThan);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.First(), 10);

            var query2 = new Query<Account>()
                .WhereGreaterThan(Account.Fields.NumberOfEmployees, 10, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.NumberOfEmployees);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.GreaterThan);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.First(), 10);
        }

        [Fact]
        public void ShouldSetWhereIn()
        {
            var query = new Query<Account>()
                .WhereIn(
                    Account.Fields.CustomerTypeCode,
                    1,
                    2,
                    3
                );

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CustomerTypeCode);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.In);
            Assert.True(query.QueryExpression.Criteria.Conditions.First().Values.Contains(1));
            Assert.True(query.QueryExpression.Criteria.Conditions.First().Values.Contains(2));
            Assert.True(query.QueryExpression.Criteria.Conditions.First().Values.Contains(3));

            var query2 = new Query<Account>()
                .WhereIn(
                    Account.EntityLogicalName,
                    Account.Fields.CustomerTypeCode,
                    1,
                    2,
                    3
                );

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CustomerTypeCode);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.In);
            Assert.True(query2.QueryExpression.Criteria.Conditions.First().Values.Contains(1));
            Assert.True(query2.QueryExpression.Criteria.Conditions.First().Values.Contains(2));
            Assert.True(query2.QueryExpression.Criteria.Conditions.First().Values.Contains(3));
        }

        [Fact]
        public void ShouldSetWhereInFiscalPeriod()
        {
            var query = new Query<Account>()
                .WhereInFiscalPeriod(Account.Fields.CreatedOn, 1);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.InFiscalPeriod);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.First(), 1);

            var query2 = new Query<Account>()
                .WhereInFiscalPeriod(Account.Fields.CreatedOn, 1, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.InFiscalPeriod);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.First(), 1);
        }

        [Fact]
        public void ShouldSetWhereInFiscalPeriodAndYear()
        {
            var query = new Query<Account>()
                .WhereInFiscalPeriodAndYear(Account.Fields.CreatedOn, 1, 2018);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.InFiscalPeriodAndYear);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values[0], 1);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values[1], 2018);

            var query2 = new Query<Account>()
                .WhereInFiscalPeriodAndYear(
                    Account.Fields.CreatedOn,
                    1,
                    2018,
                    Account.EntityLogicalName
                );

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.InFiscalPeriodAndYear);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values[0], 1);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values[1], 2018);
        }

        [Fact]
        public void ShouldSetWhereInFiscalYear()
        {
            var query = new Query<Account>()
                .WhereInFiscalYear(Account.Fields.CreatedOn, 2018);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.InFiscalYear);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.First(), 2018);

            var query2 = new Query<Account>()
                .WhereInFiscalYear(Account.Fields.CreatedOn, 2018, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.InFiscalYear);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.First(), 2018);
        }

        [Fact]
        public void ShouldSetWhereInList()
        {
            var query = new Query<Account>()
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
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.In);
            Assert.True(query.QueryExpression.Criteria.Conditions.First().Values is IList);

            var query2 = new Query<Account>()
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
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.In);
            Assert.True(query.QueryExpression.Criteria.Conditions.First().Values is IList);
        }

        [Fact]
        public void ShouldSetWhereInOrAfterFiscalPeriodAndYear()
        {
            var query = new Query<Account>()
                .WhereInOrAfterFiscalPeriodAndYear(Account.Fields.CreatedOn, 1, 2018);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.InOrAfterFiscalPeriodAndYear);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values[0], 1);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values[1], 2018);

            var query2 = new Query<Account>()
                .WhereInOrAfterFiscalPeriodAndYear(
                    Account.Fields.CreatedOn,
                    1,
                    2018,
                    Account.EntityLogicalName
                );

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.InOrAfterFiscalPeriodAndYear);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values[0], 1);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values[1], 2018);
        }

        [Fact]
        public void ShouldSetWhereInOrBeforeFiscalPeriodAndYear()
        {
            var query = new Query<Account>()
                .WhereInOrBeforeFiscalPeriodAndYear(Account.Fields.CreatedOn, 1, 2018);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.InOrBeforeFiscalPeriodAndYear);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values[0], 1);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values[1], 2018);

            var query2 = new Query<Account>()
                .WhereInOrBeforeFiscalPeriodAndYear(
                    Account.Fields.CreatedOn,
                    1,
                    2018,
                    Account.EntityLogicalName
                );

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.InOrBeforeFiscalPeriodAndYear);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values[0], 1);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values[1], 2018);
        }

        [Fact]
        public void ShouldSetWhereLast7Days()
        {
            var query = new Query<Account>()
                .WhereLast7Days(Account.Fields.CreatedOn);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.Last7Days);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.Count, 0);

            var query2 = new Query<Account>()
                .WhereLast7Days(Account.Fields.CreatedOn, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.Last7Days);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.Count, 0);
        }

        [Fact]
        public void ShouldSetWhereLastFiscalPeriod()
        {
            var query = new Query<Account>()
                .WhereLastFiscalPeriod(Account.Fields.CreatedOn);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.LastFiscalPeriod);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.Count, 0);

            var query2 = new Query<Account>()
                .WhereLastFiscalPeriod(Account.Fields.CreatedOn, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.LastFiscalPeriod);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.Count, 0);
        }

        [Fact]
        public void ShouldSetWhereLastFiscalYear()
        {
            var query = new Query<Account>()
                .WhereLastFiscalYear(Account.Fields.CreatedOn);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.LastFiscalYear);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.Count, 0);

            var query2 = new Query<Account>()
                .WhereLastFiscalYear(Account.Fields.CreatedOn, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.LastFiscalYear);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.Count, 0);
        }

        [Fact]
        public void ShouldSetWhereLastMonth()
        {
            var query = new Query<Account>()
                .WhereLastMonth(Account.Fields.CreatedOn);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.LastMonth);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.Count, 0);

            var query2 = new Query<Account>()
                .WhereLastMonth(Account.Fields.CreatedOn, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.LastMonth);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.Count, 0);
        }

        [Fact]
        public void ShouldSetWhereLastWeek()
        {
            var query = new Query<Account>()
                .WhereLastWeek(Account.Fields.CreatedOn);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.LastWeek);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.Count, 0);

            var query2 = new Query<Account>()
                .WhereLastWeek(Account.Fields.CreatedOn, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.LastWeek);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.Count, 0);
        }

        [Fact]
        public void ShouldSetWhereLastXDays()
        {
            var query = new Query<Account>()
                .WhereLastXDays(Account.Fields.CreatedOn, 10);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.LastXDays);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.First(), 10);

            var query2 = new Query<Account>()
                .WhereLastXDays(Account.Fields.CreatedOn, 10, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.LastXDays);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.First(), 10);
        }

        [Fact]
        public void ShouldSetWhereLastXFiscalPeriods()
        {
            var query = new Query<Account>()
                .WhereLastXFiscalPeriods(Account.Fields.CreatedOn, 10);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.LastXFiscalPeriods);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.First(), 10);

            var query2 = new Query<Account>()
                .WhereLastXFiscalPeriods(Account.Fields.CreatedOn, 10, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.LastXFiscalPeriods);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.First(), 10);
        }

        [Fact]
        public void ShouldSetWhereLastXFiscalYears()
        {
            var query = new Query<Account>()
                .WhereLastXFiscalYears(Account.Fields.CreatedOn, 10);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.LastXFiscalYears);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.First(), 10);

            var query2 = new Query<Account>()
                .WhereLastXFiscalYears(Account.Fields.CreatedOn, 10, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.LastXFiscalYears);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.First(), 10);
        }

        [Fact]
        public void ShouldSetWhereLastXHours()
        {
            var query = new Query<Account>()
                .WhereLastXHours(Account.Fields.CreatedOn, 10);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.LastXHours);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.First(), 10);

            var query2 = new Query<Account>()
                .WhereLastXHours(Account.Fields.CreatedOn, 10, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.LastXHours);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.First(), 10);
        }

        [Fact]
        public void ShouldSetWhereLastXMonths()
        {
            var query = new Query<Account>()
                .WhereLastXMonths(Account.Fields.CreatedOn, 10);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.LastXMonths);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.First(), 10);

            var query2 = new Query<Account>()
                .WhereLastXMonths(Account.Fields.CreatedOn, 10, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.LastXMonths);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.First(), 10);
        }

        [Fact]
        public void ShouldSetWhereLastXWeeks()
        {
            var query = new Query<Account>()
                .WhereLastXWeeks(Account.Fields.CreatedOn, 10);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.LastXWeeks);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.First(), 10);

            var query2 = new Query<Account>()
                .WhereLastXWeeks(Account.Fields.CreatedOn, 10, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.LastXWeeks);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.First(), 10);
        }

        [Fact]
        public void ShouldSetWhereLastXYears()
        {
            var query = new Query<Account>()
                .WhereLastXYears(Account.Fields.CreatedOn, 10);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.LastXYears);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.First(), 10);

            var query2 = new Query<Account>()
                .WhereLastXYears(Account.Fields.CreatedOn, 10, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.LastXYears);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.First(), 10);
        }

        [Fact]
        public void ShouldSetWhereLastYear()
        {
            var query = new Query<Account>()
                .WhereLastYear(Account.Fields.CreatedOn);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.LastYear);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.Count, 0);

            var query2 = new Query<Account>()
                .WhereLastYear(Account.Fields.CreatedOn, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.LastYear);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.Count, 0);
        }

        [Fact]
        public void ShouldSetWhereLessEqual()
        {
            var query = new Query<Account>()
                .WhereLessEqual(Account.Fields.NumberOfEmployees, 10);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.NumberOfEmployees);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.LessEqual);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.First(), 10);

            var query2 = new Query<Account>()
                .WhereLessEqual(Account.Fields.NumberOfEmployees, 10, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.NumberOfEmployees);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.LessEqual);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.First(), 10);
        }

        [Fact]
        public void ShouldSetWhereLessThan()
        {
            var query = new Query<Account>()
                .WhereLessThan(Account.Fields.NumberOfEmployees, 10);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.NumberOfEmployees);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.LessThan);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.First(), 10);

            var query2 = new Query<Account>()
                .WhereLessThan(Account.Fields.NumberOfEmployees, 10, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.NumberOfEmployees);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.LessThan);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.First(), 10);
        }

        [Fact]
        public void ShouldSetWhereLike()
        {
            var query = new Query<Account>()
                .WhereLike(Account.Fields.Name, "%test%");

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.Like);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.First(), "%test%");

            var query2 = new Query<Account>()
                .WhereLike(Account.Fields.Name, "%test%", Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.Like);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.First(), "%test%");
        }

        [Fact]
        public void ShouldSetWhereMask()
        {
            var obj = new object();
            var query = new Query<Account>()
                .WhereMask(Account.Fields.Name, obj);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.Mask);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.First(), obj);

            var query2 = new Query<Account>()
                .WhereMask(Account.Fields.Name, obj, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.Mask);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.First(), obj);
        }

        [Fact]
        public void ShouldSetWhereMasksSelect()
        {
            var obj = new object();
            var query = new Query<Account>()
                .WhereMasksSelect(Account.Fields.Name, obj);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.MasksSelect);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.First(), obj);

            var query2 = new Query<Account>()
                .WhereMasksSelect(Account.Fields.Name, obj, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.MasksSelect);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.First(), obj);
        }

        [Fact]
        public void ShouldSetWhereNext7Days()
        {
            var query = new Query<Account>()
                .WhereNext7Days(Account.Fields.CreatedOn);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.Next7Days);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.Count, 0);

            var query2 = new Query<Account>()
                .WhereNext7Days(Account.Fields.CreatedOn, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.Next7Days);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.Count, 0);
        }

        [Fact]
        public void ShouldSetWhereNextFiscalPeriod()
        {
            var query = new Query<Account>()
                .WhereNextFiscalPeriod(Account.Fields.CreatedOn);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.NextFiscalPeriod);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.Count, 0);

            var query2 = new Query<Account>()
                .WhereNextFiscalPeriod(Account.Fields.CreatedOn, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.NextFiscalPeriod);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.Count, 0);
        }

        [Fact]
        public void ShouldSetWhereNextFiscalYear()
        {
            var query = new Query<Account>()
                .WhereNextFiscalYear(Account.Fields.CreatedOn);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.NextFiscalYear);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.Count, 0);

            var query2 = new Query<Account>()
                .WhereNextFiscalYear(Account.Fields.CreatedOn, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.NextFiscalYear);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.Count, 0);
        }

        [Fact]
        public void ShouldSetWhereNextMonth()
        {
            var query = new Query<Account>()
                .WhereNextMonth(Account.Fields.CreatedOn);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.NextMonth);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.Count, 0);

            var query2 = new Query<Account>()
                .WhereNextMonth(Account.Fields.CreatedOn, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.NextMonth);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.Count, 0);
        }

        [Fact]
        public void ShouldSetWhereNextWeek()
        {
            var query = new Query<Account>()
                .WhereNextWeek(Account.Fields.CreatedOn);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.NextWeek);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.Count, 0);

            var query2 = new Query<Account>()
                .WhereNextWeek(Account.Fields.CreatedOn, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.NextWeek);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.Count, 0);
        }

        [Fact]
        public void ShouldSetWhereNextXDays()
        {
            var query = new Query<Account>()
                .WhereNextXDays(Account.Fields.CreatedOn, 10);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.NextXDays);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.First(), 10);

            var query2 = new Query<Account>()
                .WhereNextXDays(Account.Fields.CreatedOn, 10, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.NextXDays);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.First(), 10);
        }

        [Fact]
        public void ShouldSetWhereNextXFiscalPeriods()
        {
            var query = new Query<Account>()
                .WhereNextXFiscalPeriods(Account.Fields.CreatedOn, 10);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.NextXFiscalPeriods);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.First(), 10);

            var query2 = new Query<Account>()
                .WhereNextXFiscalPeriods(Account.Fields.CreatedOn, 10, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.NextXFiscalPeriods);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.First(), 10);
        }

        [Fact]
        public void ShouldSetWhereNextXFiscalYears()
        {
            var query = new Query<Account>()
                .WhereNextXFiscalYears(Account.Fields.CreatedOn, 10);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.NextXFiscalYears);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.First(), 10);

            var query2 = new Query<Account>()
                .WhereNextXFiscalYears(Account.Fields.CreatedOn, 10, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.NextXFiscalYears);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.First(), 10);
        }

        [Fact]
        public void ShouldSetWhereNextXHours()
        {
            var query = new Query<Account>()
                .WhereNextXHours(Account.Fields.CreatedOn, 10);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.NextXHours);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.First(), 10);

            var query2 = new Query<Account>()
                .WhereNextXHours(Account.Fields.CreatedOn, 10, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.NextXHours);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.First(), 10);
        }

        [Fact]
        public void ShouldSetWhereNextXMonths()
        {
            var query = new Query<Account>()
                .WhereNextXMonths(Account.Fields.CreatedOn, 10);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.NextXMonths);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.First(), 10);

            var query2 = new Query<Account>()
                .WhereNextXMonths(Account.Fields.CreatedOn, 10, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.NextXMonths);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.First(), 10);
        }

        [Fact]
        public void ShouldSetWhereNextXWeeks()
        {
            var query = new Query<Account>()
                .WhereNextXWeeks(Account.Fields.CreatedOn, 10);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.NextXWeeks);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.First(), 10);

            var query2 = new Query<Account>()
                .WhereNextXWeeks(Account.Fields.CreatedOn, 10, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.NextXWeeks);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.First(), 10);
        }

        [Fact]
        public void ShouldSetWhereNextXYears()
        {
            var query = new Query<Account>()
                .WhereNextXYears(Account.Fields.CreatedOn, 10);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.NextXYears);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.First(), 10);

            var query2 = new Query<Account>()
                .WhereNextXYears(Account.Fields.CreatedOn, 10, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.NextXYears);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.First(), 10);
        }

        [Fact]
        public void ShouldSetWhereNextYear()
        {
            var query = new Query<Account>()
                .WhereNextYear(Account.Fields.CreatedOn);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.NextYear);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.Count, 0);

            var query2 = new Query<Account>()
                .WhereNextYear(Account.Fields.CreatedOn, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.NextYear);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.Count, 0);
        }

        [Fact]
        public void ShouldSetWhereNotBetween()
        {
            var query = new Query<Account>()
                .WhereNotBetween(Account.Fields.NumberOfEmployees, 10, 50);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.NumberOfEmployees);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.NotBetween);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values[0], 10);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values[1], 50);

            var query2 = new Query<Account>()
                .WhereNotBetween(
                    Account.Fields.NumberOfEmployees,
                    10,
                    50,
                    Account.EntityLogicalName
                );

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.NumberOfEmployees);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.NotBetween);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values[0], 10);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values[1], 50);
        }

        [Fact]
        public void ShouldSetWhereNotEqual()
        {
            var query = new Query<Account>()
                .WhereNotEqual(Account.Fields.Name, "test");

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.NotEqual);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.First(), "test");

            var query2 = new Query<Account>()
                .WhereNotEqual(Account.Fields.Name, "test", Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.NotEqual);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.First(), "test");
        }

        [Fact]
        public void ShouldSetWhereNotEqualBusinessId()
        {
            var query = new Query<Account>()
                .WhereNotEqualBusinessId(Account.Fields.OwningBusinessUnit);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.OwningBusinessUnit);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.NotEqualBusinessId);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.Count, 0);

            var query2 = new Query<Account>()
                .WhereNotEqualBusinessId(Account.Fields.OwningBusinessUnit, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.OwningBusinessUnit);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.NotEqualBusinessId);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.Count, 0);
        }

        [Fact]
        public void ShouldSetWhereNotEqualUserId()
        {
            var query = new Query<Account>()
                .WhereNotEqualUserId(Account.Fields.OwnerId);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.OwnerId);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.NotEqualUserId);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.Count, 0);

            var query2 = new Query<Account>()
                .WhereNotEqualUserId(Account.Fields.OwnerId, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.OwnerId);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.NotEqualUserId);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.Count, 0);
        }

        [Fact]
        public void ShouldSetWhereNotIn()
        {
            var query = new Query<Account>()
                .WhereNotIn(
                    Account.Fields.CustomerTypeCode,
                    1,
                    2,
                    3
                );

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CustomerTypeCode);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.NotIn);
            Assert.True(query.QueryExpression.Criteria.Conditions.First().Values.Contains(1));
            Assert.True(query.QueryExpression.Criteria.Conditions.First().Values.Contains(2));
            Assert.True(query.QueryExpression.Criteria.Conditions.First().Values.Contains(3));

            var query2 = new Query<Account>()
                .WhereNotIn(
                    Account.EntityLogicalName,
                    Account.Fields.CustomerTypeCode,
                    1,
                    2,
                    3
                );

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CustomerTypeCode);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.NotIn);
            Assert.True(query2.QueryExpression.Criteria.Conditions.First().Values.Contains(1));
            Assert.True(query2.QueryExpression.Criteria.Conditions.First().Values.Contains(2));
            Assert.True(query2.QueryExpression.Criteria.Conditions.First().Values.Contains(3));
        }

        [Fact]
        public void ShouldSetWhereNotInList()
        {
            var query = new Query<Account>()
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
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.NotIn);
            Assert.True(query.QueryExpression.Criteria.Conditions.First().Values is IList);

            var query2 = new Query<Account>()
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
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.NotIn);
            Assert.True(query.QueryExpression.Criteria.Conditions.First().Values is IList);
        }

        [Fact]
        public void ShouldSetWhereNotLike()
        {
            var query = new Query<Account>()
                .WhereNotLike(Account.Fields.Name, "%test%");

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.NotLike);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.First(), "%test%");

            var query2 = new Query<Account>()
                .WhereNotLike(Account.Fields.Name, "%test%", Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.NotLike);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.First(), "%test%");
        }

        [Fact]
        public void ShouldSetWhereNotMask()
        {
            var obj = new object();
            var query = new Query<Account>()
                .WhereNotMask(Account.Fields.Name, obj);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.NotMask);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.First(), obj);

            var query2 = new Query<Account>()
                .WhereNotMask(Account.Fields.Name, obj, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.NotMask);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.First(), obj);
        }

        [Fact]
        public void ShouldSetWhereNotNull()
        {
            var query = new Query<Account>()
                .WhereNotNull(Account.Fields.Name);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.NotNull);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.Count, 0);

            var query2 = new Query<Account>()
                .WhereNotNull(Account.Fields.Name, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.NotNull);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.Count, 0);
        }

        [Fact]
        public void ShouldSetWhereNotOn()
        {
            var date = new DateTime();
            var query = new Query<Account>()
                .WhereNotOn(Account.Fields.CreatedOn, date);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.NotOn);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.First(), date);

            var query2 = new Query<Account>()
                .WhereNotOn(Account.Fields.CreatedOn, date, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.NotOn);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.First(), date);
        }

        [Fact]
        public void ShouldSetWhereNotUnder()
        {
            var guid = new Guid();
            var query = new Query<Account>()
                .WhereNotUnder(Account.Fields.AccountId, guid);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.AccountId);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.NotUnder);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.First(), guid);

            var query2 = new Query<Account>()
                .WhereNotUnder(Account.Fields.AccountId, guid, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.AccountId);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.NotUnder);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.First(), guid);
        }

        [Fact]
        public void ShouldSetWhereNull()
        {
            var query = new Query<Account>()
                .WhereNull(Account.Fields.Name);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.Null);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.Count, 0);

            var query2 = new Query<Account>()
                .WhereNull(Account.Fields.Name, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.Null);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.Count, 0);
        }

        [Fact]
        public void ShouldSetWhereOlderThanXDays()
        {
            var query = new Query<Account>()
                .WhereOlderThanXDays(Account.Fields.CreatedOn, 10);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.OlderThanXDays);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.First(), 10);

            var query2 = new Query<Account>()
                .WhereOlderThanXDays(Account.Fields.CreatedOn, 10, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.OlderThanXDays);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.First(), 10);
        }

        [Fact]
        public void ShouldSetWhereOlderThanXHours()
        {
            var query = new Query<Account>()
                .WhereOlderThanXHours(Account.Fields.CreatedOn, 10);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.OlderThanXHours);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.First(), 10);

            var query2 = new Query<Account>()
                .WhereOlderThanXHours(Account.Fields.CreatedOn, 10, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.OlderThanXHours);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.First(), 10);
        }

        [Fact]
        public void ShouldSetWhereOlderThanXMinutes()
        {
            var query = new Query<Account>()
                .WhereOlderThanXMinutes(Account.Fields.CreatedOn, 10);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.OlderThanXMinutes);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.First(), 10);

            var query2 = new Query<Account>()
                .WhereOlderThanXMinutes(Account.Fields.CreatedOn, 10, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.OlderThanXMinutes);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.First(), 10);
        }

        [Fact]
        public void ShouldSetWhereOlderThanXMonths()
        {
            var query = new Query<Account>()
                .WhereOlderThanXMonths(Account.Fields.CreatedOn, 10);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.OlderThanXMonths);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.First(), 10);

            var query2 = new Query<Account>()
                .WhereOlderThanXMonths(Account.Fields.CreatedOn, 10, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.OlderThanXMonths);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.First(), 10);
        }

        [Fact]
        public void ShouldSetWhereOlderThanXWeeks()
        {
            var query = new Query<Account>()
                .WhereOlderThanXWeeks(Account.Fields.CreatedOn, 10);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.OlderThanXWeeks);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.First(), 10);

            var query2 = new Query<Account>()
                .WhereOlderThanXWeeks(Account.Fields.CreatedOn, 10, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.OlderThanXWeeks);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.First(), 10);
        }

        [Fact]
        public void ShouldSetWhereOlderThanXYears()
        {
            var query = new Query<Account>()
                .WhereOlderThanXYears(Account.Fields.CreatedOn, 10);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.OlderThanXYears);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.First(), 10);

            var query2 = new Query<Account>()
                .WhereOlderThanXYears(Account.Fields.CreatedOn, 10, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.OlderThanXYears);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.First(), 10);
        }

        [Fact]
        public void ShouldSetWhereOn()
        {
            var date = new DateTime();
            var query = new Query<Account>()
                .WhereOn(Account.Fields.CreatedOn, date);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.On);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.First(), date);

            var query2 = new Query<Account>()
                .WhereOn(Account.Fields.CreatedOn, date, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.On);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.First(), date);
        }

        [Fact]
        public void ShouldSetWhereOnOrAfter()
        {
            var date = new DateTime();
            var query = new Query<Account>()
                .WhereOnOrAfter(Account.Fields.CreatedOn, date);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.OnOrAfter);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.First(), date);

            var query2 = new Query<Account>()
                .WhereOnOrAfter(Account.Fields.CreatedOn, date, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.OnOrAfter);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.First(), date);
        }

        [Fact]
        public void ShouldSetWhereOnOrBefore()
        {
            var date = new DateTime();
            var query = new Query<Account>()
                .WhereOnOrBefore(Account.Fields.CreatedOn, date);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.OnOrBefore);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.First(), date);

            var query2 = new Query<Account>()
                .WhereOnOrBefore(Account.Fields.CreatedOn, date, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.OnOrBefore);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.First(), date);
        }

        [Fact]
        public void ShouldSetWhereThisFiscalPeriod()
        {
            var query = new Query<Account>()
                .WhereThisFiscalPeriod(Account.Fields.CreatedOn);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.ThisFiscalPeriod);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.Count, 0);

            var query2 = new Query<Account>()
                .WhereThisFiscalPeriod(Account.Fields.CreatedOn, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.ThisFiscalPeriod);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.Count, 0);
        }

        [Fact]
        public void ShouldSetWhereThisFiscalYear()
        {
            var query = new Query<Account>()
                .WhereThisFiscalYear(Account.Fields.CreatedOn);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.ThisFiscalYear);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.Count, 0);

            var query2 = new Query<Account>()
                .WhereThisFiscalYear(Account.Fields.CreatedOn, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.ThisFiscalYear);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.Count, 0);
        }

        [Fact]
        public void ShouldSetWhereThisMonth()
        {
            var query = new Query<Account>()
                .WhereThisMonth(Account.Fields.CreatedOn);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.ThisMonth);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.Count, 0);

            var query2 = new Query<Account>()
                .WhereThisMonth(Account.Fields.CreatedOn, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.ThisMonth);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.Count, 0);
        }

        [Fact]
        public void ShouldSetWhereThisWeek()
        {
            var query = new Query<Account>()
                .WhereThisWeek(Account.Fields.CreatedOn);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.ThisWeek);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.Count, 0);

            var query2 = new Query<Account>()
                .WhereThisWeek(Account.Fields.CreatedOn, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.ThisWeek);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.Count, 0);
        }

        [Fact]
        public void ShouldSetWhereThisYear()
        {
            var query = new Query<Account>()
                .WhereThisYear(Account.Fields.CreatedOn);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.ThisYear);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.Count, 0);

            var query2 = new Query<Account>()
                .WhereThisYear(Account.Fields.CreatedOn, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.ThisYear);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.Count, 0);
        }

        [Fact]
        public void ShouldSetWhereToday()
        {
            var query = new Query<Account>()
                .WhereToday(Account.Fields.CreatedOn);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.Today);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.Count, 0);

            var query2 = new Query<Account>()
                .WhereToday(Account.Fields.CreatedOn, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.Today);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.Count, 0);
        }

        [Fact]
        public void ShouldSetWhereTomorrow()
        {
            var query = new Query<Account>()
                .WhereTomorrow(Account.Fields.CreatedOn);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.Tomorrow);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.Count, 0);

            var query2 = new Query<Account>()
                .WhereTomorrow(Account.Fields.CreatedOn, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.Tomorrow);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.Count, 0);
        }

        [Fact]
        public void ShouldSetWhereUnder()
        {
            var guid = new Guid();
            var query = new Query<Account>()
                .WhereUnder(Account.Fields.AccountId, guid);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.AccountId);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.Under);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.First(), guid);

            var query2 = new Query<Account>()
                .WhereUnder(Account.Fields.AccountId, guid, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.AccountId);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.Under);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.First(), guid);
        }

        [Fact]
        public void ShouldSetWhereUnderOrEqual()
        {
            var guid = new Guid();
            var query = new Query<Account>()
                .WhereUnderOrEqual(Account.Fields.AccountId, guid);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.AccountId);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.UnderOrEqual);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.First(), guid);

            var query2 = new Query<Account>()
                .WhereUnderOrEqual(Account.Fields.AccountId, guid, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.AccountId);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.UnderOrEqual);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.First(), guid);
        }

        [Fact]
        public void ShouldSetWhereYesterday()
        {
            var query = new Query<Account>()
                .WhereYesterday(Account.Fields.CreatedOn);

            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.Yesterday);
            Assert.Equal(query.QueryExpression.Criteria.Conditions.First().Values.Count, 0);

            var query2 = new Query<Account>()
                .WhereYesterday(Account.Fields.CreatedOn, Account.EntityLogicalName);

            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Operator, ConditionOperator.Yesterday);
            Assert.Equal(query2.QueryExpression.Criteria.Conditions.First().Values.Count, 0);
        }

        #endregion Conditions

        #region IOrganizationService calls

        private readonly Guid item1Id = Guid.NewGuid();
        private readonly Guid item2Id = Guid.NewGuid();

        [Fact]
        public void ShouldGetAll()
        {
            var fakedContext = new XrmFakedContext { ProxyTypesAssembly = typeof(Account).Assembly };
            fakedContext.Initialize(new Account { Id = item1Id });
            var service = fakedContext.GetOrganizationService();

            var records = new Query<Account>()
                .GetAll(service);

            records.Should().HaveCount(1);
            records.First().Id.Should().Be(item1Id);
        }

        [Fact]
        public void ShouldGetAllTopCount()
        {
            var fakedContext = new XrmFakedContext { ProxyTypesAssembly = typeof(Account).Assembly };
            fakedContext.Initialize(new Account { Id = item1Id });
            var service = fakedContext.GetOrganizationService();

            var records = new Query<Account>()
                          .Top(100)
                          .GetAll(service);

            Assert.Equal(records.Count, 1);
            Assert.Equal(records.First().Id, item1Id);
        }

        [Fact]
        public void ShouldGetAllWithExtension()
        {
            var fakedContext = new XrmFakedContext { ProxyTypesAssembly = typeof(Account).Assembly };
            fakedContext.Initialize(new Account { Id = item1Id });
            var service = fakedContext.GetOrganizationService();

            var records = service.RetrieveMultiple(new Query<Account>());

            Assert.Equal(records.Count, 1);
            Assert.Equal(records.First().Id, item1Id);
        }

        [Fact]
        public void ShouldGetAllWithoutPaging()
        {
            var fakedContext = new XrmFakedContext { ProxyTypesAssembly = typeof(Account).Assembly };
            fakedContext.Initialize(new Account { Id = item1Id });
            var service = fakedContext.GetOrganizationService();

            var records = new Query<Account>()
                .GetAll(service);

            Assert.Equal(records.Count, 1);
            Assert.Equal(records.First().Id, item1Id);
        }

        [Fact]
        public void ShouldGetById()
        {
            var fakedContext = new XrmFakedContext { ProxyTypesAssembly = typeof(Account).Assembly };
            fakedContext.Initialize(new Account { Id = item1Id });
            var service = fakedContext.GetOrganizationService();

            var query = new Query<Account>();
            var record = query.GetById(Guid.NewGuid(), service);

            Assert.NotNull(record);
            Assert.Equal(Account.EntityLogicalName + "id", query.QueryExpression.Criteria.Conditions.First().AttributeName);
        }

        [Fact]
        public void ShouldGetByIdForActivity()
        {
            var fakedContext = new XrmFakedContext { ProxyTypesAssembly = typeof(Account).Assembly };
            fakedContext.Initialize(new Account { Id = item1Id });

            var service = fakedContext.GetOrganizationService();

            var query = new Query<Task>();
            var record = query.GetById(Guid.NewGuid(), service, true);

            Assert.NotNull(record);
            Assert.Equal("activityid", query.QueryExpression.Criteria.Conditions.First().AttributeName);
        }

        [Fact]
        public void ShouldGetFirst()
        {
            var fakedContext = new XrmFakedContext { ProxyTypesAssembly = typeof(Account).Assembly };
            fakedContext.Initialize(new List<Entity> { new Account { Id = item1Id }, new Account { Id = item2Id } });
            var service = fakedContext.GetOrganizationService();

            var record = new Query<Account>()
                .GetFirst(service);

            Assert.NotNull(record);
            Assert.Equal(record.Id, item1Id);
        }

        [Fact]
        public void ShouldGetFirstNull()
        {
            var fakedContext = new XrmFakedContext { ProxyTypesAssembly = typeof(Account).Assembly };
            var service = fakedContext.GetOrganizationService();

            var ex = Record.Exception(() => new Query<Account>().GetFirst(service));

            Assert.IsType<InvalidOperationException>(ex);
        }

        [Fact]
        public void ShouldGetFirstOrDefault()
        {
            var fakedContext = new XrmFakedContext { ProxyTypesAssembly = typeof(Account).Assembly };
            fakedContext.Initialize(new List<Entity> { new Account { Id = item1Id }, new Account { Id = item2Id } });
            var service = fakedContext.GetOrganizationService();

            var record = new Query<Account>()
                .GetFirstOrDefault(service);

            Assert.NotNull(record);
            Assert.Equal(record.Id, item1Id);
        }

        [Fact]
        public void ShouldGetFirstOrDefaultIsNull()
        {
            var fakedContext = new XrmFakedContext { ProxyTypesAssembly = typeof(Account).Assembly };
            var service = fakedContext.GetOrganizationService();

            var record = new Query<Account>()
                .GetFirstOrDefault(service);

            Assert.Null(record);
        }

        [Fact]
        public void ShouldGetLast()
        {
            var fakedContext = new XrmFakedContext { ProxyTypesAssembly = typeof(Account).Assembly };
            fakedContext.Initialize(new List<Entity> { new Account { Id = item1Id }, new Account { Id = item2Id } });
            var service = fakedContext.GetOrganizationService();

            var record = new Query<Account>()
                .GetLast(service);

            Assert.NotNull(record);
            Assert.Equal(record.Id, item2Id);
        }

        [Fact]
        public void ShouldGetLastNull()
        {
            var fakedContext = new XrmFakedContext { ProxyTypesAssembly = typeof(Account).Assembly };
            var service = fakedContext.GetOrganizationService();

            var ex = Record.Exception(() => new Query<Account>().GetLast(service));

            Assert.IsType<InvalidOperationException>(ex);
        }

        [Fact]
        public void ShouldGetLastOrDefault()
        {
            var fakedContext = new XrmFakedContext { ProxyTypesAssembly = typeof(Account).Assembly };
            fakedContext.Initialize(new List<Entity> { new Account { Id = item1Id }, new Account { Id = item2Id } });
            var service = fakedContext.GetOrganizationService();

            var record = new Query<Account>()
                .GetLastOrDefault(service);

            Assert.NotNull(record);
            Assert.Equal(record.Id, item2Id);
        }

        [Fact]
        public void ShouldGetLastOrDefaultIsNull()
        {
            var fakedContext = new XrmFakedContext { ProxyTypesAssembly = typeof(Account).Assembly };
            var service = fakedContext.GetOrganizationService();

            var record = new Query<Account>()
                .GetLastOrDefault(service);

            Assert.Null(record);
        }

        [Fact]
        public void ShouldGetResults()
        {
            var fakedContext = new XrmFakedContext { ProxyTypesAssembly = typeof(Account).Assembly };
            fakedContext.Initialize(new List<Entity> { new Account { Id = item1Id }, new Account { Id = item2Id } });
            var service = fakedContext.GetOrganizationService();

            var records = new Query<Account>()
                .GetResults(service);

            Assert.Equal(2, records.Entities.Count);
        }

        [Fact]
        public void ShouldGetSingle()
        {
            var fakedContext = new XrmFakedContext { ProxyTypesAssembly = typeof(Account).Assembly };
            fakedContext.Initialize(new List<Entity> { new Account { Id = item1Id } });
            var service = fakedContext.GetOrganizationService();

            var record = new Query<Account>()
                .GetSingle(service);

            Assert.NotNull(record);
            Assert.Equal(record.Id, item1Id);
        }

        [Fact]
        public void ShouldGetSingleMany()
        {
            var fakedContext = new XrmFakedContext { ProxyTypesAssembly = typeof(Account).Assembly };
            fakedContext.Initialize(new List<Entity> { new Account { Id = item1Id }, new Account { Id = item2Id } });
            var service = fakedContext.GetOrganizationService();

            var ex = Record.Exception(() => new Query<Account>().GetSingle(service));

            Assert.IsType<InvalidOperationException>(ex);
        }

        [Fact]
        public void ShouldGetSingleNull()
        {
            var fakedContext = new XrmFakedContext { ProxyTypesAssembly = typeof(Account).Assembly };
            var service = fakedContext.GetOrganizationService();

            var ex = Record.Exception(() => new Query<Account>().GetSingle(service));

            Assert.IsType<InvalidOperationException>(ex);
        }

        [Fact]
        public void ShouldGetSingleOrDefault()
        {
            var fakedContext = new XrmFakedContext { ProxyTypesAssembly = typeof(Account).Assembly };
            fakedContext.Initialize(new List<Entity> { new Account { Id = item1Id } });
            var service = fakedContext.GetOrganizationService();

            var record = new Query<Account>()
                .GetSingleOrDefault(service);

            Assert.NotNull(record);
            Assert.Equal(record.Id, item1Id);
        }

        [Fact]
        public void ShouldGetSingleOrDefaultIsNull()
        {
            var fakedContext = new XrmFakedContext { ProxyTypesAssembly = typeof(Account).Assembly };
            fakedContext.Initialize(new List<Entity> { new Account { Id = item1Id }, new Account { Id = item2Id } });
            var service = fakedContext.GetOrganizationService();

            var record = new Query<Account>()
                .GetSingleOrDefault(service);

            Assert.Null(record);
        }

        #endregion IOrganizationService calls
    }
}
