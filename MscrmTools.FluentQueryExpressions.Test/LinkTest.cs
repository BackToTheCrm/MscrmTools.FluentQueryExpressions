using Microsoft.Xrm.Sdk.Query;
using MscrmTools.FluentQueryExpressions.Test.AppCode;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace MscrmTools.FluentQueryExpressions.Test
{
    public class LinkTest
    {
        [Fact]
        public void ShouldAddFilter()
        {
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId, JoinOperator.LeftOuter)
                    .AddFilters(LogicalOperator.Or, new Filter()));

            Assert.Equal(LogicalOperator.Or, query.QueryExpression.LinkEntities.First().LinkCriteria.FilterOperator);
            Assert.Single(query.QueryExpression.LinkEntities.First().LinkCriteria.Filters);
        }

        [Fact]
        public void ShouldAddFilters()
        {
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId, JoinOperator.LeftOuter)
                    .AddFilters(
                        new Filter(),
                        new Filter()
                    )
                );

            Assert.Equal(LogicalOperator.And, query.QueryExpression.LinkEntities.First().LinkCriteria.FilterOperator);
            Assert.Equal(2, query.QueryExpression.LinkEntities.First().LinkCriteria.Filters.Count);
        }

        [Fact]
        public void ShouldAddLink()
        {
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId, JoinOperator.LeftOuter)
                    .AddLink(new Link<Task>(Task.Fields.RegardingObjectId, Contact.Fields.ContactId)));

            Assert.Single(query.QueryExpression.LinkEntities.First().LinkEntities);
        }

        [Fact]
        public void ShouldAddLink2()
        {
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId, JoinOperator.LeftOuter)
                    .AddLink(new Link(Task.EntityLogicalName, Task.Fields.RegardingObjectId, Contact.Fields.ContactId)));

            Assert.Single(query.QueryExpression.LinkEntities.First().LinkEntities);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkEntities.First().LinkFromEntityName, Contact.EntityLogicalName);
        }

        [Fact]
        public void ShouldAddOrder()
        {
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId,
                        JoinOperator.LeftOuter)
                    .Order(Contact.Fields.FullName, OrderType.Ascending));

            Assert.Equal(query.QueryExpression.LinkEntities.First().Orders.First().AttributeName, Contact.Fields.FullName);
            Assert.Equal(OrderType.Ascending, query.QueryExpression.LinkEntities.First().Orders.First().OrderType);
        }

        [Fact]
        public void ShouldCreateLink()
        {
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId, JoinOperator.LeftOuter));

            Assert.Single(query.QueryExpression.LinkEntities);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkFromEntityName, Account.EntityLogicalName);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkFromAttributeName, Account.Fields.AccountId);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkToAttributeName, Contact.Fields.ParentCustomerId);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkToEntityName, Contact.EntityLogicalName);
            Assert.Equal(query.QueryExpression.LinkEntities.First().EntityAlias, Contact.EntityLogicalName);
            Assert.Equal(JoinOperator.LeftOuter, query.QueryExpression.LinkEntities.First().JoinOperator);
        }

        [Fact]
        public void ShouldCreateLink2()
        {
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId, JoinOperator.LeftOuter)
                    .SetAlias("toto")
                    .SetDefaultFilterOperator(LogicalOperator.Or)
                    .Select(true));

            Assert.Single(query.QueryExpression.LinkEntities);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkFromEntityName, Account.EntityLogicalName);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkFromAttributeName, Account.Fields.AccountId);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkToAttributeName, Contact.Fields.ParentCustomerId);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkToEntityName, Contact.EntityLogicalName);
            Assert.Equal("toto", query.QueryExpression.LinkEntities.First().EntityAlias);
            Assert.Equal(JoinOperator.LeftOuter, query.QueryExpression.LinkEntities.First().JoinOperator);
            Assert.Equal(LogicalOperator.Or, query.QueryExpression.LinkEntities.First().LinkCriteria.FilterOperator);
            Assert.True(query.QueryExpression.LinkEntities.First().Columns.AllColumns);
        }

        [Fact]
        public void ShouldCreateLink3()
        {
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId, JoinOperator.LeftOuter)
                    .SetAlias("toto")
                    .SetDefaultFilterOperator(LogicalOperator.Or)
                    .Select());

            Assert.Single(query.QueryExpression.LinkEntities);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkFromEntityName, Account.EntityLogicalName);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkFromAttributeName, Account.Fields.AccountId);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkToAttributeName, Contact.Fields.ParentCustomerId);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkToEntityName, Contact.EntityLogicalName);
            Assert.Equal("toto", query.QueryExpression.LinkEntities.First().EntityAlias);
            Assert.Equal(JoinOperator.LeftOuter, query.QueryExpression.LinkEntities.First().JoinOperator);
            Assert.Equal(LogicalOperator.Or, query.QueryExpression.LinkEntities.First().LinkCriteria.FilterOperator);
            Assert.Empty(query.QueryExpression.LinkEntities.First().Columns.Columns);
        }

        [Fact]
        public void ShouldCreateLink4()
        {
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId, JoinOperator.LeftOuter)
                    .SetAlias("toto")
                    .SetDefaultFilterOperator(LogicalOperator.Or)
                    .Select(Contact.Fields.FullName));

            Assert.Single(query.QueryExpression.LinkEntities);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkFromEntityName, Account.EntityLogicalName);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkFromAttributeName, Account.Fields.AccountId);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkToAttributeName, Contact.Fields.ParentCustomerId);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkToEntityName, Contact.EntityLogicalName);
            Assert.Equal("toto", query.QueryExpression.LinkEntities.First().EntityAlias);
            Assert.Equal(JoinOperator.LeftOuter, query.QueryExpression.LinkEntities.First().JoinOperator);
            Assert.Equal(LogicalOperator.Or, query.QueryExpression.LinkEntities.First().LinkCriteria.FilterOperator);
            Assert.Equal(query.QueryExpression.LinkEntities.First().Columns.Columns.First(), Contact.Fields.FullName);
        }

        [Fact]
        public void ShouldCreateLinkNotGeneric()
        {
            var query = new Query<Account>(null)
                .AddLink(new Link(Contact.EntityLogicalName, Contact.Fields.ParentCustomerId, Account.Fields.AccountId, JoinOperator.LeftOuter));

            Assert.Single(query.QueryExpression.LinkEntities);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkFromEntityName, Account.EntityLogicalName);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkFromAttributeName, Account.Fields.AccountId);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkToAttributeName, Contact.Fields.ParentCustomerId);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkToEntityName, Contact.EntityLogicalName);
            Assert.Equal(query.QueryExpression.LinkEntities.First().EntityAlias, Contact.EntityLogicalName);
            Assert.Equal(JoinOperator.LeftOuter, query.QueryExpression.LinkEntities.First().JoinOperator);
        }

        #region Conditions

        [Fact]
        public void ShouldSetWhere()
        {
            var guid = Guid.NewGuid();
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).Where(Account.Fields.AccountId, ConditionOperator.Above, guid));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.AccountId);
            Assert.Equal(ConditionOperator.Above, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Single(), guid);

            var query2 = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).Where(Account.EntityLogicalName, Account.Fields.AccountId, ConditionOperator.Above, guid));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.AccountId);
            Assert.Equal(ConditionOperator.Above, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Single(), guid);
        }

        [Fact]
        public void ShouldSetWhereAbove()
        {
            var guid = Guid.NewGuid();
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereAbove(Account.Fields.AccountId, guid));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.AccountId);
            Assert.Equal(ConditionOperator.Above, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Single(), guid);

            var query2 = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereAbove(Account.Fields.AccountId, guid, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.AccountId);
            Assert.Equal(ConditionOperator.Above, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Single(), guid);
        }

        [Fact]
        public void ShouldSetWhereAboveOrEqual()
        {
            var guid = Guid.NewGuid();
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereAboveOrEqual(Account.Fields.AccountId, guid));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.AccountId);
            Assert.Equal(ConditionOperator.AboveOrEqual, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Single(), guid);

            var query2 = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereAboveOrEqual(Account.Fields.AccountId, guid, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.AccountId);
            Assert.Equal(ConditionOperator.AboveOrEqual, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Single(), guid);
        }

        [Fact]
        public void ShouldSetWhereBeginsWith()
        {
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereBeginsWith(Account.Fields.Name, "test"));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(ConditionOperator.BeginsWith, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal("test", query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Single());

            var query2 = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereBeginsWith(Account.Fields.Name, "test", Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(ConditionOperator.BeginsWith, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal("test", query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Single());
        }

        [Fact]
        public void ShouldSetWhereBetween()
        {
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereBetween(Account.Fields.NumberOfEmployees, 10, 50));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.NumberOfEmployees);
            Assert.Equal(ConditionOperator.Between, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal(10, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values[0]);
            Assert.Equal(50, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values[1]);

            var query2 = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereBetween(Account.Fields.NumberOfEmployees, 10, 50, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.NumberOfEmployees);
            Assert.Equal(ConditionOperator.Between, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal(10, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values[0]);
            Assert.Equal(50, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values[1]);
        }

        [Fact]
        public void ShouldSetWhereChildOf()
        {
            var guid = Guid.NewGuid();
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereChildOf(Account.Fields.AccountId, guid));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.AccountId);
            Assert.Equal(ConditionOperator.ChildOf, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Single(), guid);

            var query2 = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereChildOf(Account.Fields.AccountId, guid, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.AccountId);
            Assert.Equal(ConditionOperator.ChildOf, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Single(), guid);
        }

        [Fact]
        public void ShouldSetWhereContains()
        {
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereContains(Account.Fields.Name, "test"));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(ConditionOperator.Contains, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal("test", query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Single());

            var query2 = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereContains(Account.Fields.Name, "test", Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(ConditionOperator.Contains, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal("test", query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Single());
        }

        [Fact]
        public void ShouldSetWhereContainValues()
        {
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereContainValues(Account.Fields.CustomerTypeCode, 1, 2, 3));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CustomerTypeCode);
            Assert.Equal(ConditionOperator.ContainValues, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Contains(1, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values);
            Assert.Contains(2, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values);
            Assert.Contains(3, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values);

            var query2 = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereContainValues(Account.EntityLogicalName, Account.Fields.CustomerTypeCode, 1, 2, 3));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CustomerTypeCode);
            Assert.Equal(ConditionOperator.ContainValues, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Contains(1, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values);
            Assert.Contains(2, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values);
            Assert.Contains(3, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereDoesNotBeginWith()
        {
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereDoesNotBeginWith(Account.Fields.Name, "test"));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(ConditionOperator.DoesNotBeginWith, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal("test", query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Single());

            var query2 = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereDoesNotBeginWith(Account.Fields.Name, "test", Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(ConditionOperator.DoesNotBeginWith, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal("test", query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Single());
        }

        [Fact]
        public void ShouldSetWhereDoesNotContain()
        {
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereDoesNotContain(Account.Fields.Name, "test"));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(ConditionOperator.DoesNotContain, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal("test", query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Single());

            var query2 = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereDoesNotContain(Account.Fields.Name, "test", Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(ConditionOperator.DoesNotContain, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal("test", query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Single());
        }

        [Fact]
        public void ShouldSetWhereDoesNotContainValues()
        {
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereDoesNotContainValues(Account.Fields.CustomerTypeCode, 1, 2, 3));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CustomerTypeCode);
            Assert.Equal(ConditionOperator.DoesNotContainValues, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Contains(1, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values);
            Assert.Contains(2, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values);
            Assert.Contains(3, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values);

            var query2 = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereDoesNotContainValues(Account.EntityLogicalName, Account.Fields.CustomerTypeCode, 1, 2, 3));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CustomerTypeCode);
            Assert.Equal(ConditionOperator.DoesNotContainValues, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Contains(1, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values);
            Assert.Contains(2, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values);
            Assert.Contains(3, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereDoesNotEndWith()
        {
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereDoesNotEndWith(Account.Fields.Name, "test"));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(ConditionOperator.DoesNotEndWith, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal("test", query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Single());

            var query2 = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereDoesNotEndWith(Account.Fields.Name, "test", Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(ConditionOperator.DoesNotEndWith, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal("test", query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Single());
        }

        [Fact]
        public void ShouldSetWhereEndsWith()
        {
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereEndsWith(Account.Fields.Name, "test"));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(ConditionOperator.EndsWith, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal("test", query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Single());

            var query2 = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereEndsWith(Account.Fields.Name, "test", Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(ConditionOperator.EndsWith, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal("test", query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Single());
        }

        [Fact]
        public void ShouldSetWhereEqual()
        {
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereEqual(Account.Fields.Name, "test"));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(ConditionOperator.Equal, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal("test", query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Single());

            var query2 = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereEqual(Account.Fields.Name, "test", Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(ConditionOperator.Equal, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal("test", query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Single());
        }

        [Fact]
        public void ShouldSetWhereEqualBusinessId()
        {
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereEqualBusinessId(Account.Fields.OwningBusinessUnit));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.OwningBusinessUnit);
            Assert.Equal(ConditionOperator.EqualBusinessId, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Empty(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values);

            var query2 = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereEqualBusinessId(Account.Fields.OwningBusinessUnit, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.OwningBusinessUnit);
            Assert.Equal(ConditionOperator.EqualBusinessId, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Empty(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereEqualUserId()
        {
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereEqualUserId(Account.Fields.OwnerId));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.OwnerId);
            Assert.Equal(ConditionOperator.EqualUserId, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Empty(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values);

            var query2 = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereEqualUserId(Account.Fields.OwnerId, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.OwnerId);
            Assert.Equal(ConditionOperator.EqualUserId, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Empty(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereEqualUserLanguage()
        {
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereEqualUserLanguage("no_language_attribute"));

            Assert.Equal("no_language_attribute", query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.EqualUserLanguage, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Empty(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values);

            var query2 = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereEqualUserLanguage("no_language_attribute", Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal("no_language_attribute", query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName);
            Assert.Equal(ConditionOperator.EqualUserLanguage, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Empty(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereEqualUserOrUserHierarchy()
        {
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereEqualUserOrUserHierarchy(Account.Fields.OwnerId));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.OwnerId);
            Assert.Equal(ConditionOperator.EqualUserOrUserHierarchy, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Empty(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values);

            var query2 = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereEqualUserOrUserHierarchy(Account.Fields.OwnerId, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.OwnerId);
            Assert.Equal(ConditionOperator.EqualUserOrUserHierarchy, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Empty(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereEqualUserOrUserHierarchyAndTeams()
        {
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereEqualUserOrUserHierarchyAndTeams(Account.Fields.OwnerId));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.OwnerId);
            Assert.Equal(ConditionOperator.EqualUserOrUserHierarchyAndTeams, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Empty(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values);

            var query2 = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereEqualUserOrUserHierarchyAndTeams(Account.Fields.OwnerId, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.OwnerId);
            Assert.Equal(ConditionOperator.EqualUserOrUserHierarchyAndTeams, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Empty(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereEqualUserOrUserTeams()
        {
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereEqualUserOrUserTeams(Account.Fields.OwnerId));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.OwnerId);
            Assert.Equal(ConditionOperator.EqualUserOrUserTeams, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Empty(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values);

            var query2 = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereEqualUserOrUserTeams(Account.Fields.OwnerId, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.OwnerId);
            Assert.Equal(ConditionOperator.EqualUserOrUserTeams, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Empty(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereEqualUserTeams()
        {
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereEqualUserTeams(Account.Fields.OwnerId));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.OwnerId);
            Assert.Equal(ConditionOperator.EqualUserTeams, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Empty(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values);

            var query2 = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereEqualUserTeams(Account.Fields.OwnerId, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.OwnerId);
            Assert.Equal(ConditionOperator.EqualUserTeams, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Empty(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereGreaterEqual()
        {
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereGreaterEqual(Account.Fields.NumberOfEmployees, 10));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.NumberOfEmployees);
            Assert.Equal(ConditionOperator.GreaterEqual, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal(10, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First());

            var query2 = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereGreaterEqual(Account.Fields.NumberOfEmployees, 10, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.NumberOfEmployees);
            Assert.Equal(ConditionOperator.GreaterEqual, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal(10, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First());
        }

        [Fact]
        public void ShouldSetWhereGreaterThan()
        {
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereGreaterThan(Account.Fields.NumberOfEmployees, 10));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.NumberOfEmployees);
            Assert.Equal(ConditionOperator.GreaterThan, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal(10, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First());

            var query2 = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereGreaterThan(Account.Fields.NumberOfEmployees, 10, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.NumberOfEmployees);
            Assert.Equal(ConditionOperator.GreaterThan, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal(10, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First());
        }

        [Fact]
        public void ShouldSetWhereIn()
        {
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereIn(Account.Fields.CustomerTypeCode, 1, 2, 3));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CustomerTypeCode);
            Assert.Equal(ConditionOperator.In, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Contains(1, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values);
            Assert.Contains(2, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values);
            Assert.Contains(3, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values);

            var query2 = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereIn(Account.EntityLogicalName, Account.Fields.CustomerTypeCode, 1, 2, 3));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CustomerTypeCode);
            Assert.Equal(ConditionOperator.In, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Contains(1, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values);
            Assert.Contains(2, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values);
            Assert.Contains(3, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereInFiscalPeriod()
        {
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereInFiscalPeriod(Account.Fields.CreatedOn, 1));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.InFiscalPeriod, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal(1, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First());

            var query2 = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereInFiscalPeriod(Account.Fields.CreatedOn, 1, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.InFiscalPeriod, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal(1, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First());
        }

        [Fact]
        public void ShouldSetWhereInFiscalPeriodAndYear()
        {
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereInFiscalPeriodAndYear(Account.Fields.CreatedOn, 1, 2018));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.InFiscalPeriodAndYear, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal(1, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values[0]);
            Assert.Equal(2018, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values[1]);

            var query2 = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereInFiscalPeriodAndYear(Account.Fields.CreatedOn, 1, 2018, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.InFiscalPeriodAndYear, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal(1, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values[0]);
            Assert.Equal(2018, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values[1]);
        }

        [Fact]
        public void ShouldSetWhereInFiscalYear()
        {
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereInFiscalYear(Account.Fields.CreatedOn, 2018));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.InFiscalYear, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal(2018, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First());

            var query2 = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereInFiscalYear(Account.Fields.CreatedOn, 2018, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.InFiscalYear, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal(2018, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First());
        }

        [Fact]
        public void ShouldSetWhereInList()
        {
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId)
                    .WhereIn(Account.Fields.CustomerTypeCode, new List<int> { 1, 2, 3 }));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CustomerTypeCode);
            Assert.Equal(ConditionOperator.In, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.IsAssignableFrom<IList>(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values);

            var query2 = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId)
                    .WhereIn(Account.EntityLogicalName, Account.Fields.CustomerTypeCode, new List<int> { 1, 2, 3 }));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CustomerTypeCode);
            Assert.Equal(ConditionOperator.In, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.IsAssignableFrom<IList>(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereInOrAfterFiscalPeriodAndYear()
        {
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereInOrAfterFiscalPeriodAndYear(Account.Fields.CreatedOn, 1, 2018));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.InOrAfterFiscalPeriodAndYear, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal(1, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values[0]);
            Assert.Equal(2018, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values[1]);

            var query2 = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereInOrAfterFiscalPeriodAndYear(Account.Fields.CreatedOn, 1, 2018, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.InOrAfterFiscalPeriodAndYear, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal(1, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values[0]);
            Assert.Equal(2018, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values[1]);
        }

        [Fact]
        public void ShouldSetWhereInOrBeforeFiscalPeriodAndYear()
        {
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereInOrBeforeFiscalPeriodAndYear(Account.Fields.CreatedOn, 1, 2018));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.InOrBeforeFiscalPeriodAndYear, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal(1, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values[0]);
            Assert.Equal(2018, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values[1]);

            var query2 = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereInOrBeforeFiscalPeriodAndYear(Account.Fields.CreatedOn, 1, 2018, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.InOrBeforeFiscalPeriodAndYear, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal(1, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values[0]);
            Assert.Equal(2018, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values[1]);
        }

        [Fact]
        public void ShouldSetWhereLast7Days()
        {
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereLast7Days(Account.Fields.CreatedOn));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.Last7Days, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Empty(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values);

            var query2 = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereLast7Days(Account.Fields.CreatedOn, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.Last7Days, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Empty(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereLastFiscalPeriod()
        {
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereLastFiscalPeriod(Account.Fields.CreatedOn));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.LastFiscalPeriod, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Empty(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values);

            var query2 = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereLastFiscalPeriod(Account.Fields.CreatedOn, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.LastFiscalPeriod, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Empty(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereLastFiscalYear()
        {
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereLastFiscalYear(Account.Fields.CreatedOn));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.LastFiscalYear, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Empty(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values);

            var query2 = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereLastFiscalYear(Account.Fields.CreatedOn, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.LastFiscalYear, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Empty(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereLastMonth()
        {
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereLastMonth(Account.Fields.CreatedOn));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.LastMonth, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Empty(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values);

            var query2 = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereLastMonth(Account.Fields.CreatedOn, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.LastMonth, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Empty(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereLastWeek()
        {
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereLastWeek(Account.Fields.CreatedOn));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.LastWeek, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Empty(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values);

            var query2 = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereLastWeek(Account.Fields.CreatedOn, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.LastWeek, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Empty(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereLastXDays()
        {
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereLastXDays(Account.Fields.CreatedOn, 10));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.LastXDays, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal(10, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First());

            var query2 = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereLastXDays(Account.Fields.CreatedOn, 10, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.LastXDays, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal(10, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First());
        }

        [Fact]
        public void ShouldSetWhereLastXFiscalPeriods()
        {
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereLastXFiscalPeriods(Account.Fields.CreatedOn, 10));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.LastXFiscalPeriods, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal(10, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First());

            var query2 = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereLastXFiscalPeriods(Account.Fields.CreatedOn, 10, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.LastXFiscalPeriods, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal(10, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First());
        }

        [Fact]
        public void ShouldSetWhereLastXFiscalYears()
        {
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereLastXFiscalYears(Account.Fields.CreatedOn, 10));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.LastXFiscalYears, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal(10, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First());

            var query2 = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereLastXFiscalYears(Account.Fields.CreatedOn, 10, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.LastXFiscalYears, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal(10, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First());
        }

        [Fact]
        public void ShouldSetWhereLastXHours()
        {
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereLastXHours(Account.Fields.CreatedOn, 10));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.LastXHours, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal(10, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First());

            var query2 = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereLastXHours(Account.Fields.CreatedOn, 10, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.LastXHours, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal(10, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First());
        }

        [Fact]
        public void ShouldSetWhereLastXMonths()
        {
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereLastXMonths(Account.Fields.CreatedOn, 10));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.LastXMonths, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal(10, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First());

            var query2 = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereLastXMonths(Account.Fields.CreatedOn, 10, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.LastXMonths, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal(10, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First());
        }

        [Fact]
        public void ShouldSetWhereLastXWeeks()
        {
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereLastXWeeks(Account.Fields.CreatedOn, 10));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.LastXWeeks, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal(10, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First());

            var query2 = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereLastXWeeks(Account.Fields.CreatedOn, 10, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.LastXWeeks, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal(10, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First());
        }

        [Fact]
        public void ShouldSetWhereLastXYears()
        {
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereLastXYears(Account.Fields.CreatedOn, 10));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.LastXYears, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal(10, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First());

            var query2 = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereLastXYears(Account.Fields.CreatedOn, 10, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.LastXYears, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal(10, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First());
        }

        [Fact]
        public void ShouldSetWhereLastYear()
        {
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereLastYear(Account.Fields.CreatedOn));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.LastYear, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Empty(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values);

            var query2 = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereLastYear(Account.Fields.CreatedOn, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.LastYear, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Empty(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereLessEqual()
        {
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereLessEqual(Account.Fields.NumberOfEmployees, 10));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.NumberOfEmployees);
            Assert.Equal(ConditionOperator.LessEqual, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal(10, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First());

            var query2 = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereLessEqual(Account.Fields.NumberOfEmployees, 10, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.NumberOfEmployees);
            Assert.Equal(ConditionOperator.LessEqual, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal(10, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First());
        }

        [Fact]
        public void ShouldSetWhereLessThan()
        {
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereLessThan(Account.Fields.NumberOfEmployees, 10));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.NumberOfEmployees);
            Assert.Equal(ConditionOperator.LessThan, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal(10, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First());

            var query2 = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereLessThan(Account.Fields.NumberOfEmployees, 10, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.NumberOfEmployees);
            Assert.Equal(ConditionOperator.LessThan, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal(10, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First());
        }

        [Fact]
        public void ShouldSetWhereLike()
        {
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereLike(Account.Fields.Name, "%test%"));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(ConditionOperator.Like, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal("%test%", query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First());

            var query2 = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereLike(Account.Fields.Name, "%test%", Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(ConditionOperator.Like, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal("%test%", query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First());
        }

        [Fact]
        public void ShouldSetWhereMask()
        {
            var obj = new object();
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereMask(Account.Fields.Name, obj));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(ConditionOperator.Mask, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), obj);

            var query2 = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereMask(Account.Fields.Name, obj, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(ConditionOperator.Mask, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), obj);
        }

        [Fact]
        public void ShouldSetWhereMasksSelect()
        {
            var obj = new object();
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereMasksSelect(Account.Fields.Name, obj));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(ConditionOperator.MasksSelect, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), obj);

            var query2 = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereMasksSelect(Account.Fields.Name, obj, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(ConditionOperator.MasksSelect, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), obj);
        }

        [Fact]
        public void ShouldSetWhereNext7Days()
        {
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNext7Days(Account.Fields.CreatedOn));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.Next7Days, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Empty(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values);

            var query2 = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNext7Days(Account.Fields.CreatedOn, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.Next7Days, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Empty(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereNextFiscalPeriod()
        {
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNextFiscalPeriod(Account.Fields.CreatedOn));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.NextFiscalPeriod, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Empty(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values);

            var query2 = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNextFiscalPeriod(Account.Fields.CreatedOn, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.NextFiscalPeriod, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Empty(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereNextFiscalYear()
        {
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNextFiscalYear(Account.Fields.CreatedOn));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.NextFiscalYear, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Empty(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values);

            var query2 = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNextFiscalYear(Account.Fields.CreatedOn, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.NextFiscalYear, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Empty(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereNextMonth()
        {
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNextMonth(Account.Fields.CreatedOn));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.NextMonth, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Empty(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values);

            var query2 = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNextMonth(Account.Fields.CreatedOn, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.NextMonth, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Empty(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereNextWeek()
        {
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNextWeek(Account.Fields.CreatedOn));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.NextWeek, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Empty(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values);

            var query2 = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNextWeek(Account.Fields.CreatedOn, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.NextWeek, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Empty(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereNextXDays()
        {
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNextXDays(Account.Fields.CreatedOn, 10));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.NextXDays, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal(10, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First());

            var query2 = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNextXDays(Account.Fields.CreatedOn, 10, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.NextXDays, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal(10, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First());
        }

        [Fact]
        public void ShouldSetWhereNextXFiscalPeriods()
        {
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNextXFiscalPeriods(Account.Fields.CreatedOn, 10));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.NextXFiscalPeriods, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal(10, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First());

            var query2 = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNextXFiscalPeriods(Account.Fields.CreatedOn, 10, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.NextXFiscalPeriods, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal(10, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First());
        }

        [Fact]
        public void ShouldSetWhereNextXFiscalYears()
        {
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNextXFiscalYears(Account.Fields.CreatedOn, 10));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.NextXFiscalYears, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal(10, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First());

            var query2 = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNextXFiscalYears(Account.Fields.CreatedOn, 10, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.NextXFiscalYears, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal(10, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First());
        }

        [Fact]
        public void ShouldSetWhereNextXHours()
        {
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNextXHours(Account.Fields.CreatedOn, 10));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.NextXHours, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal(10, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First());

            var query2 = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNextXHours(Account.Fields.CreatedOn, 10, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.NextXHours, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal(10, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First());
        }

        [Fact]
        public void ShouldSetWhereNextXMonths()
        {
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNextXMonths(Account.Fields.CreatedOn, 10));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.NextXMonths, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal(10, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First());

            var query2 = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNextXMonths(Account.Fields.CreatedOn, 10, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.NextXMonths, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal(10, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First());
        }

        [Fact]
        public void ShouldSetWhereNextXWeeks()
        {
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNextXWeeks(Account.Fields.CreatedOn, 10));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.NextXWeeks, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal(10, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First());

            var query2 = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNextXWeeks(Account.Fields.CreatedOn, 10, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.NextXWeeks, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal(10, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First());
        }

        [Fact]
        public void ShouldSetWhereNextXYears()
        {
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNextXYears(Account.Fields.CreatedOn, 10));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.NextXYears, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal(10, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First());

            var query2 = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNextXYears(Account.Fields.CreatedOn, 10, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.NextXYears, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal(10, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First());
        }

        [Fact]
        public void ShouldSetWhereNextYear()
        {
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNextYear(Account.Fields.CreatedOn));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.NextYear, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Empty(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values);

            var query2 = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNextYear(Account.Fields.CreatedOn, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.NextYear, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Empty(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereNotBetween()
        {
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNotBetween(Account.Fields.NumberOfEmployees, 10, 50));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.NumberOfEmployees);
            Assert.Equal(ConditionOperator.NotBetween, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal(10, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values[0]);
            Assert.Equal(50, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values[1]);

            var query2 = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNotBetween(Account.Fields.NumberOfEmployees, 10, 50, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.NumberOfEmployees);
            Assert.Equal(ConditionOperator.NotBetween, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal(10, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values[0]);
            Assert.Equal(50, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values[1]);
        }

        [Fact]
        public void ShouldSetWhereNotEqual()
        {
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNotEqual(Account.Fields.Name, "test"));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(ConditionOperator.NotEqual, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal("test", query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First());

            var query2 = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNotEqual(Account.Fields.Name, "test", Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(ConditionOperator.NotEqual, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal("test", query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First());
        }

        [Fact]
        public void ShouldSetWhereNotEqualBusinessId()
        {
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNotEqualBusinessId(Account.Fields.OwningBusinessUnit));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.OwningBusinessUnit);
            Assert.Equal(ConditionOperator.NotEqualBusinessId, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Empty(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values);

            var query2 = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNotEqualBusinessId(Account.Fields.OwningBusinessUnit, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.OwningBusinessUnit);
            Assert.Equal(ConditionOperator.NotEqualBusinessId, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Empty(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereNotEqualUserId()
        {
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNotEqualUserId(Account.Fields.OwnerId));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.OwnerId);
            Assert.Equal(ConditionOperator.NotEqualUserId, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Empty(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values);

            var query2 = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNotEqualUserId(Account.Fields.OwnerId, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.OwnerId);
            Assert.Equal(ConditionOperator.NotEqualUserId, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Empty(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereNotIn()
        {
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNotIn(Account.Fields.CustomerTypeCode, 1, 2, 3));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CustomerTypeCode);
            Assert.Equal(ConditionOperator.NotIn, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Contains(1, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values);
            Assert.Contains(2, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values);
            Assert.Contains(3, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values);

            var query2 = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNotIn(Account.EntityLogicalName, Account.Fields.CustomerTypeCode, 1, 2, 3));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CustomerTypeCode);
            Assert.Equal(ConditionOperator.NotIn, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Contains(1, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values);
            Assert.Contains(2, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values);
            Assert.Contains(3, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereNotInList()
        {
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId)
                    .WhereNotIn(Account.Fields.CustomerTypeCode, new List<int> { 1, 2, 3 }));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CustomerTypeCode);
            Assert.Equal(ConditionOperator.NotIn, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.IsAssignableFrom<IList>(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values);

            var query2 = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId)
                    .WhereNotIn(Account.EntityLogicalName, Account.Fields.CustomerTypeCode, new List<int> { 1, 2, 3 }));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CustomerTypeCode);
            Assert.Equal(ConditionOperator.NotIn, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.IsAssignableFrom<IList>(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereNotLike()
        {
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNotLike(Account.Fields.Name, "%test%"));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(ConditionOperator.NotLike, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal("%test%", query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First());

            var query2 = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNotLike(Account.Fields.Name, "%test%", Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(ConditionOperator.NotLike, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal("%test%", query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First());
        }

        [Fact]
        public void ShouldSetWhereNotMask()
        {
            var obj = new object();
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNotMask(Account.Fields.Name, obj));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(ConditionOperator.NotMask, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), obj);

            var query2 = new Query<Account>(null)
                 .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNotMask(Account.Fields.Name, obj, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(ConditionOperator.NotMask, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), obj);
        }

        [Fact]
        public void ShouldSetWhereNotNull()
        {
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNotNull(Account.Fields.Name));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(ConditionOperator.NotNull, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Empty(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values);

            var query2 = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNotNull(Account.Fields.Name, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(ConditionOperator.NotNull, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Empty(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereNotOn()
        {
            var date = new DateTime();
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNotOn(Account.Fields.CreatedOn, date));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.NotOn, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), date);

            var query2 = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNotOn(Account.Fields.CreatedOn, date, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.NotOn, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), date);
        }

        [Fact]
        public void ShouldSetWhereNotUnder()
        {
            var guid = new Guid();
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNotUnder(Account.Fields.AccountId, guid));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.AccountId);
            Assert.Equal(ConditionOperator.NotUnder, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), guid);

            var query2 = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNotUnder(Account.Fields.AccountId, guid, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.AccountId);
            Assert.Equal(ConditionOperator.NotUnder, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), guid);
        }

        [Fact]
        public void ShouldSetWhereNull()
        {
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNull(Account.Fields.Name));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(ConditionOperator.Null, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Empty(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values);

            var query2 = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNull(Account.Fields.Name, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(ConditionOperator.Null, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Empty(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereOlderThanXDays()
        {
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereOlderThanXDays(Account.Fields.CreatedOn, 10));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.OlderThanXDays, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal(10, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First());

            var query2 = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereOlderThanXDays(Account.Fields.CreatedOn, 10, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.OlderThanXDays, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal(10, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First());
        }

        [Fact]
        public void ShouldSetWhereOlderThanXHours()
        {
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereOlderThanXHours(Account.Fields.CreatedOn, 10));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.OlderThanXHours, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal(10, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First());

            var query2 = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereOlderThanXHours(Account.Fields.CreatedOn, 10, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.OlderThanXHours, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal(10, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First());
        }

        [Fact]
        public void ShouldSetWhereOlderThanXMinutes()
        {
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereOlderThanXMinutes(Account.Fields.CreatedOn, 10));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.OlderThanXMinutes, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal(10, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First());

            var query2 = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereOlderThanXMinutes(Account.Fields.CreatedOn, 10, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.OlderThanXMinutes, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal(10, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First());
        }

        [Fact]
        public void ShouldSetWhereOlderThanXMonths()
        {
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereOlderThanXMonths(Account.Fields.CreatedOn, 10));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.OlderThanXMonths, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal(10, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First());

            var query2 = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereOlderThanXMonths(Account.Fields.CreatedOn, 10, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.OlderThanXMonths, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal(10, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First());
        }

        [Fact]
        public void ShouldSetWhereOlderThanXWeeks()
        {
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereOlderThanXWeeks(Account.Fields.CreatedOn, 10));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.OlderThanXWeeks, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal(10, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First());

            var query2 = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereOlderThanXWeeks(Account.Fields.CreatedOn, 10, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.OlderThanXWeeks, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal(10, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First());
        }

        [Fact]
        public void ShouldSetWhereOlderThanXYears()
        {
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereOlderThanXYears(Account.Fields.CreatedOn, 10));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.OlderThanXYears, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal(10, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First());

            var query2 = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereOlderThanXYears(Account.Fields.CreatedOn, 10, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.OlderThanXYears, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal(10, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First());
        }

        [Fact]
        public void ShouldSetWhereOn()
        {
            var date = new DateTime();
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereOn(Account.Fields.CreatedOn, date));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.On, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), date);

            var query2 = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereOn(Account.Fields.CreatedOn, date, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.On, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), date);
        }

        [Fact]
        public void ShouldSetWhereOnOrAfter()
        {
            var date = new DateTime();
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereOnOrAfter(Account.Fields.CreatedOn, date));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.OnOrAfter, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), date);

            var query2 = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereOnOrAfter(Account.Fields.CreatedOn, date, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.OnOrAfter, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), date);
        }

        [Fact]
        public void ShouldSetWhereOnOrBefore()
        {
            var date = new DateTime();
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereOnOrBefore(Account.Fields.CreatedOn, date));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.OnOrBefore, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), date);

            var query2 = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereOnOrBefore(Account.Fields.CreatedOn, date, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.OnOrBefore, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), date);
        }

        [Fact]
        public void ShouldSetWhereThisFiscalPeriod()
        {
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereThisFiscalPeriod(Account.Fields.CreatedOn));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.ThisFiscalPeriod, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Empty(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values);

            var query2 = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereThisFiscalPeriod(Account.Fields.CreatedOn, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.ThisFiscalPeriod, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Empty(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereThisFiscalYear()
        {
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereThisFiscalYear(Account.Fields.CreatedOn));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.ThisFiscalYear, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Empty(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values);

            var query2 = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereThisFiscalYear(Account.Fields.CreatedOn, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.ThisFiscalYear, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Empty(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereThisMonth()
        {
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereThisMonth(Account.Fields.CreatedOn));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.ThisMonth, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Empty(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values);

            var query2 = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereThisMonth(Account.Fields.CreatedOn, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.ThisMonth, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Empty(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereThisWeek()
        {
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereThisWeek(Account.Fields.CreatedOn));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.ThisWeek, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Empty(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values);

            var query2 = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereThisWeek(Account.Fields.CreatedOn, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.ThisWeek, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Empty(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereThisYear()
        {
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereThisYear(Account.Fields.CreatedOn));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.ThisYear, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Empty(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values);

            var query2 = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereThisYear(Account.Fields.CreatedOn, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.ThisYear, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Empty(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereToday()
        {
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereToday(Account.Fields.CreatedOn));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.Today, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Empty(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values);

            var query2 = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereToday(Account.Fields.CreatedOn, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.Today, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Empty(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereTomorrow()
        {
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereTomorrow(Account.Fields.CreatedOn));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.Tomorrow, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Empty(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values);

            var query2 = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereTomorrow(Account.Fields.CreatedOn, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.Tomorrow, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Empty(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values);
        }

        [Fact]
        public void ShouldSetWhereUnder()
        {
            var guid = new Guid();
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereUnder(Account.Fields.AccountId, guid));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.AccountId);
            Assert.Equal(ConditionOperator.Under, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), guid);

            var query2 = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereUnder(Account.Fields.AccountId, guid, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.AccountId);
            Assert.Equal(ConditionOperator.Under, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), guid);
        }

        [Fact]
        public void ShouldSetWhereUnderOrEqual()
        {
            var guid = new Guid();
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereUnderOrEqual(Account.Fields.AccountId, guid));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.AccountId);
            Assert.Equal(ConditionOperator.UnderOrEqual, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), guid);

            var query2 = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereUnderOrEqual(Account.Fields.AccountId, guid, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.AccountId);
            Assert.Equal(ConditionOperator.UnderOrEqual, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), guid);
        }

        [Fact]
        public void ShouldSetWhereYesterday()
        {
            var query = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereYesterday(Account.Fields.CreatedOn));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.Yesterday, query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Empty(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values);

            var query2 = new Query<Account>(null)
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereYesterday(Account.Fields.CreatedOn, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(ConditionOperator.Yesterday, query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator);
            Assert.Empty(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values);
        }

        #endregion Conditions
    }
}