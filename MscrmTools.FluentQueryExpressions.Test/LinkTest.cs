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
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId, JoinOperator.LeftOuter)
                    .AddFilters(LogicalOperator.Or, new Filter()));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.FilterOperator, LogicalOperator.Or);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Filters.Count, 1);
        }

        [Fact]
        public void ShouldAddFilters()
        {
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId, JoinOperator.LeftOuter)
                    .AddFilters(
                        new Filter(),
                        new Filter()
                    )
                );

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.FilterOperator, LogicalOperator.And);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Filters.Count, 2);
        }

        [Fact]
        public void ShouldAddLink()
        {
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId, JoinOperator.LeftOuter)
                    .AddLink(new Link<Task>(Task.Fields.RegardingObjectId, Contact.Fields.ContactId)));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkEntities.Count, 1);
        }

        [Fact]
        public void ShouldAddLink2()
        {
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId, JoinOperator.LeftOuter)
                    .AddLink(new Link(Task.EntityLogicalName, Task.Fields.RegardingObjectId, Contact.Fields.ContactId)));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkEntities.Count, 1);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkEntities.First().LinkFromEntityName, Contact.EntityLogicalName);
        }

        [Fact]
        public void ShouldAddOrder()
        {
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId,
                        JoinOperator.LeftOuter)
                    .Order(Contact.Fields.FullName, OrderType.Ascending));

            Assert.Equal(query.QueryExpression.LinkEntities.First().Orders.First().AttributeName, Contact.Fields.FullName);
            Assert.Equal(query.QueryExpression.LinkEntities.First().Orders.First().OrderType, OrderType.Ascending);
        }

        [Fact]
        public void ShouldCreateLink()
        {
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId, JoinOperator.LeftOuter));

            Assert.Equal(query.QueryExpression.LinkEntities.Count, 1);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkFromEntityName, Account.EntityLogicalName);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkFromAttributeName, Account.Fields.AccountId);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkToAttributeName, Contact.Fields.ParentCustomerId);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkToEntityName, Contact.EntityLogicalName);
            Assert.Equal(query.QueryExpression.LinkEntities.First().EntityAlias, Contact.EntityLogicalName);
            Assert.Equal(query.QueryExpression.LinkEntities.First().JoinOperator, JoinOperator.LeftOuter);
        }

        [Fact]
        public void ShouldCreateLink2()
        {
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId, JoinOperator.LeftOuter)
                    .SetAlias("toto")
                    .SetDefaultFilterOperator(LogicalOperator.Or)
                    .Select(true));

            Assert.Equal(query.QueryExpression.LinkEntities.Count, 1);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkFromEntityName, Account.EntityLogicalName);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkFromAttributeName, Account.Fields.AccountId);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkToAttributeName, Contact.Fields.ParentCustomerId);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkToEntityName, Contact.EntityLogicalName);
            Assert.Equal(query.QueryExpression.LinkEntities.First().EntityAlias, "toto");
            Assert.Equal(query.QueryExpression.LinkEntities.First().JoinOperator, JoinOperator.LeftOuter);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.FilterOperator, LogicalOperator.Or);
            Assert.Equal(query.QueryExpression.LinkEntities.First().Columns.AllColumns, true);
        }

        [Fact]
        public void ShouldCreateLink3()
        {
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId, JoinOperator.LeftOuter)
                    .SetAlias("toto")
                    .SetDefaultFilterOperator(LogicalOperator.Or)
                    .Select());

            Assert.Equal(query.QueryExpression.LinkEntities.Count, 1);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkFromEntityName, Account.EntityLogicalName);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkFromAttributeName, Account.Fields.AccountId);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkToAttributeName, Contact.Fields.ParentCustomerId);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkToEntityName, Contact.EntityLogicalName);
            Assert.Equal(query.QueryExpression.LinkEntities.First().EntityAlias, "toto");
            Assert.Equal(query.QueryExpression.LinkEntities.First().JoinOperator, JoinOperator.LeftOuter);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.FilterOperator, LogicalOperator.Or);
            Assert.Equal(query.QueryExpression.LinkEntities.First().Columns.Columns.Count, 0);
        }

        [Fact]
        public void ShouldCreateLink4()
        {
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId, JoinOperator.LeftOuter)
                    .SetAlias("toto")
                    .SetDefaultFilterOperator(LogicalOperator.Or)
                    .Select(Contact.Fields.FullName));

            Assert.Equal(query.QueryExpression.LinkEntities.Count, 1);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkFromEntityName, Account.EntityLogicalName);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkFromAttributeName, Account.Fields.AccountId);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkToAttributeName, Contact.Fields.ParentCustomerId);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkToEntityName, Contact.EntityLogicalName);
            Assert.Equal(query.QueryExpression.LinkEntities.First().EntityAlias, "toto");
            Assert.Equal(query.QueryExpression.LinkEntities.First().JoinOperator, JoinOperator.LeftOuter);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.FilterOperator, LogicalOperator.Or);
            Assert.Equal(query.QueryExpression.LinkEntities.First().Columns.Columns.First(), Contact.Fields.FullName);
        }

        [Fact]
        public void ShouldCreateLinkNotGeneric()
        {
            var query = new Query<Account>()
                .AddLink(new Link(Contact.EntityLogicalName, Contact.Fields.ParentCustomerId, Account.Fields.AccountId, JoinOperator.LeftOuter));

            Assert.Equal(query.QueryExpression.LinkEntities.Count, 1);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkFromEntityName, Account.EntityLogicalName);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkFromAttributeName, Account.Fields.AccountId);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkToAttributeName, Contact.Fields.ParentCustomerId);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkToEntityName, Contact.EntityLogicalName);
            Assert.Equal(query.QueryExpression.LinkEntities.First().EntityAlias, Contact.EntityLogicalName);
            Assert.Equal(query.QueryExpression.LinkEntities.First().JoinOperator, JoinOperator.LeftOuter);
        }

        #region Conditions

        [Fact]
        public void ShouldSetWhere()
        {
            var guid = Guid.NewGuid();
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).Where(Account.Fields.AccountId, ConditionOperator.Above, guid));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.AccountId);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.Above);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Single(), guid);

            var query2 = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).Where(Account.EntityLogicalName, Account.Fields.AccountId, ConditionOperator.Above, guid));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.AccountId);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.Above);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Single(), guid);
        }

        [Fact]
        public void ShouldSetWhereAbove()
        {
            var guid = Guid.NewGuid();
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereAbove(Account.Fields.AccountId, guid));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.AccountId);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.Above);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Single(), guid);

            var query2 = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereAbove(Account.Fields.AccountId, guid, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.AccountId);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.Above);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Single(), guid);
        }

        [Fact]
        public void ShouldSetWhereAboveOrEqual()
        {
            var guid = Guid.NewGuid();
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereAboveOrEqual(Account.Fields.AccountId, guid));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.AccountId);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.AboveOrEqual);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Single(), guid);

            var query2 = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereAboveOrEqual(Account.Fields.AccountId, guid, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.AccountId);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.AboveOrEqual);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Single(), guid);
        }

        [Fact]
        public void ShouldSetWhereBeginsWith()
        {
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereBeginsWith(Account.Fields.Name, "test"));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.BeginsWith);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Single(), "test");

            var query2 = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereBeginsWith(Account.Fields.Name, "test", Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.BeginsWith);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Single(), "test");
        }

        [Fact]
        public void ShouldSetWhereBetween()
        {
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereBetween(Account.Fields.NumberOfEmployees, 10, 50));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.NumberOfEmployees);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.Between);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values[0], 10);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values[1], 50);

            var query2 = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereBetween(Account.Fields.NumberOfEmployees, 10, 50, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.NumberOfEmployees);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.Between);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values[0], 10);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values[1], 50);
        }

        [Fact]
        public void ShouldSetWhereChildOf()
        {
            var guid = Guid.NewGuid();
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereChildOf(Account.Fields.AccountId, guid));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.AccountId);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.ChildOf);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Single(), guid);

            var query2 = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereChildOf(Account.Fields.AccountId, guid, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.AccountId);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.ChildOf);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Single(), guid);
        }

        [Fact]
        public void ShouldSetWhereContains()
        {
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereContains(Account.Fields.Name, "test"));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.Contains);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Single(), "test");

            var query2 = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereContains(Account.Fields.Name, "test", Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.Contains);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Single(), "test");
        }

        [Fact]
        public void ShouldSetWhereContainValues()
        {
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereContainValues(Account.Fields.CustomerTypeCode, 1, 2, 3));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CustomerTypeCode);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.ContainValues);
            Assert.True(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Contains(1));
            Assert.True(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Contains(2));
            Assert.True(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Contains(3));

            var query2 = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereContainValues(Account.EntityLogicalName, Account.Fields.CustomerTypeCode, 1, 2, 3));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CustomerTypeCode);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.ContainValues);
            Assert.True(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Contains(1));
            Assert.True(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Contains(2));
            Assert.True(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Contains(3));
        }

        [Fact]
        public void ShouldSetWhereDoesNotBeginWith()
        {
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereDoesNotBeginWith(Account.Fields.Name, "test"));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.DoesNotBeginWith);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Single(), "test");

            var query2 = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereDoesNotBeginWith(Account.Fields.Name, "test", Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.DoesNotBeginWith);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Single(), "test");
        }

        [Fact]
        public void ShouldSetWhereDoesNotContain()
        {
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereDoesNotContain(Account.Fields.Name, "test"));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.DoesNotContain);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Single(), "test");

            var query2 = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereDoesNotContain(Account.Fields.Name, "test", Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.DoesNotContain);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Single(), "test");
        }

        [Fact]
        public void ShouldSetWhereDoesNotContainValues()
        {
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereDoesNotContainValues(Account.Fields.CustomerTypeCode, 1, 2, 3));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CustomerTypeCode);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.DoesNotContainValues);
            Assert.True(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Contains(1));
            Assert.True(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Contains(2));
            Assert.True(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Contains(3));

            var query2 = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereDoesNotContainValues(Account.EntityLogicalName, Account.Fields.CustomerTypeCode, 1, 2, 3));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CustomerTypeCode);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.DoesNotContainValues);
            Assert.True(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Contains(1));
            Assert.True(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Contains(2));
            Assert.True(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Contains(3));
        }

        [Fact]
        public void ShouldSetWhereDoesNotEndWith()
        {
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereDoesNotEndWith(Account.Fields.Name, "test"));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.DoesNotEndWith);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Single(), "test");

            var query2 = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereDoesNotEndWith(Account.Fields.Name, "test", Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.DoesNotEndWith);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Single(), "test");
        }

        [Fact]
        public void ShouldSetWhereEndsWith()
        {
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereEndsWith(Account.Fields.Name, "test"));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.EndsWith);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Single(), "test");

            var query2 = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereEndsWith(Account.Fields.Name, "test", Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.EndsWith);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Single(), "test");
        }

        [Fact]
        public void ShouldSetWhereEqual()
        {
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereEqual(Account.Fields.Name, "test"));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.Equal);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Single(), "test");

            var query2 = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereEqual(Account.Fields.Name, "test", Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.Equal);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Single(), "test");
        }

        [Fact]
        public void ShouldSetWhereEqualBusinessId()
        {
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereEqualBusinessId(Account.Fields.OwningBusinessUnit));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.OwningBusinessUnit);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.EqualBusinessId);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Count, 0);

            var query2 = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereEqualBusinessId(Account.Fields.OwningBusinessUnit, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.OwningBusinessUnit);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.EqualBusinessId);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Count, 0);
        }

        [Fact]
        public void ShouldSetWhereEqualUserId()
        {
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereEqualUserId(Account.Fields.OwnerId));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.OwnerId);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.EqualUserId);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Count, 0);

            var query2 = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereEqualUserId(Account.Fields.OwnerId, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.OwnerId);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.EqualUserId);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Count, 0);
        }

        [Fact]
        public void ShouldSetWhereEqualUserLanguage()
        {
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereEqualUserLanguage("no_language_attribute"));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, "no_language_attribute");
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.EqualUserLanguage);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Count, 0);

            var query2 = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereEqualUserLanguage("no_language_attribute", Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, "no_language_attribute");
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.EqualUserLanguage);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Count, 0);
        }

        [Fact]
        public void ShouldSetWhereEqualUserOrUserHierarchy()
        {
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereEqualUserOrUserHierarchy(Account.Fields.OwnerId));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.OwnerId);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.EqualUserOrUserHierarchy);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Count, 0);

            var query2 = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereEqualUserOrUserHierarchy(Account.Fields.OwnerId, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.OwnerId);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.EqualUserOrUserHierarchy);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Count, 0);
        }

        [Fact]
        public void ShouldSetWhereEqualUserOrUserHierarchyAndTeams()
        {
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereEqualUserOrUserHierarchyAndTeams(Account.Fields.OwnerId));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.OwnerId);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.EqualUserOrUserHierarchyAndTeams);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Count, 0);

            var query2 = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereEqualUserOrUserHierarchyAndTeams(Account.Fields.OwnerId, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.OwnerId);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.EqualUserOrUserHierarchyAndTeams);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Count, 0);
        }

        [Fact]
        public void ShouldSetWhereEqualUserOrUserTeams()
        {
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereEqualUserOrUserTeams(Account.Fields.OwnerId));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.OwnerId);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.EqualUserOrUserTeams);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Count, 0);

            var query2 = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereEqualUserOrUserTeams(Account.Fields.OwnerId, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.OwnerId);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.EqualUserOrUserTeams);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Count, 0);
        }

        [Fact]
        public void ShouldSetWhereEqualUserTeams()
        {
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereEqualUserTeams(Account.Fields.OwnerId));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.OwnerId);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.EqualUserTeams);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Count, 0);

            var query2 = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereEqualUserTeams(Account.Fields.OwnerId, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.OwnerId);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.EqualUserTeams);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Count, 0);
        }

        [Fact]
        public void ShouldSetWhereGreaterEqual()
        {
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereGreaterEqual(Account.Fields.NumberOfEmployees, 10));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.NumberOfEmployees);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.GreaterEqual);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), 10);

            var query2 = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereGreaterEqual(Account.Fields.NumberOfEmployees, 10, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.NumberOfEmployees);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.GreaterEqual);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), 10);
        }

        [Fact]
        public void ShouldSetWhereGreaterThan()
        {
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereGreaterThan(Account.Fields.NumberOfEmployees, 10));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.NumberOfEmployees);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.GreaterThan);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), 10);

            var query2 = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereGreaterThan(Account.Fields.NumberOfEmployees, 10, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.NumberOfEmployees);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.GreaterThan);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), 10);
        }

        [Fact]
        public void ShouldSetWhereIn()
        {
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereIn(Account.Fields.CustomerTypeCode, 1, 2, 3));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CustomerTypeCode);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.In);
            Assert.True(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Contains(1));
            Assert.True(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Contains(2));
            Assert.True(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Contains(3));

            var query2 = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereIn(Account.EntityLogicalName, Account.Fields.CustomerTypeCode, 1, 2, 3));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CustomerTypeCode);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.In);
            Assert.True(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Contains(1));
            Assert.True(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Contains(2));
            Assert.True(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Contains(3));
        }

        [Fact]
        public void ShouldSetWhereInFiscalPeriod()
        {
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereInFiscalPeriod(Account.Fields.CreatedOn, 1));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.InFiscalPeriod);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), 1);

            var query2 = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereInFiscalPeriod(Account.Fields.CreatedOn, 1, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.InFiscalPeriod);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), 1);
        }

        [Fact]
        public void ShouldSetWhereInFiscalPeriodAndYear()
        {
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereInFiscalPeriodAndYear(Account.Fields.CreatedOn, 1, 2018));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.InFiscalPeriodAndYear);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values[0], 1);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values[1], 2018);

            var query2 = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereInFiscalPeriodAndYear(Account.Fields.CreatedOn, 1, 2018, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.InFiscalPeriodAndYear);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values[0], 1);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values[1], 2018);
        }

        [Fact]
        public void ShouldSetWhereInFiscalYear()
        {
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereInFiscalYear(Account.Fields.CreatedOn, 2018));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.InFiscalYear);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), 2018);

            var query2 = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereInFiscalYear(Account.Fields.CreatedOn, 2018, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.InFiscalYear);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), 2018);
        }

        [Fact]
        public void ShouldSetWhereInList()
        {
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId)
                    .WhereIn(Account.Fields.CustomerTypeCode, new List<int> { 1, 2, 3 }));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CustomerTypeCode);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.In);
            Assert.True(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values is IList);

            var query2 = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId)
                    .WhereIn(Account.EntityLogicalName, Account.Fields.CustomerTypeCode, new List<int> { 1, 2, 3 }));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CustomerTypeCode);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.In);
            Assert.True(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values is IList);
        }

        [Fact]
        public void ShouldSetWhereInOrAfterFiscalPeriodAndYear()
        {
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereInOrAfterFiscalPeriodAndYear(Account.Fields.CreatedOn, 1, 2018));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.InOrAfterFiscalPeriodAndYear);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values[0], 1);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values[1], 2018);

            var query2 = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereInOrAfterFiscalPeriodAndYear(Account.Fields.CreatedOn, 1, 2018, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.InOrAfterFiscalPeriodAndYear);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values[0], 1);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values[1], 2018);
        }

        [Fact]
        public void ShouldSetWhereInOrBeforeFiscalPeriodAndYear()
        {
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereInOrBeforeFiscalPeriodAndYear(Account.Fields.CreatedOn, 1, 2018));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.InOrBeforeFiscalPeriodAndYear);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values[0], 1);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values[1], 2018);

            var query2 = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereInOrBeforeFiscalPeriodAndYear(Account.Fields.CreatedOn, 1, 2018, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.InOrBeforeFiscalPeriodAndYear);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values[0], 1);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values[1], 2018);
        }

        [Fact]
        public void ShouldSetWhereLast7Days()
        {
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereLast7Days(Account.Fields.CreatedOn));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.Last7Days);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Count, 0);

            var query2 = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereLast7Days(Account.Fields.CreatedOn, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.Last7Days);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Count, 0);
        }

        [Fact]
        public void ShouldSetWhereLastFiscalPeriod()
        {
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereLastFiscalPeriod(Account.Fields.CreatedOn));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.LastFiscalPeriod);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Count, 0);

            var query2 = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereLastFiscalPeriod(Account.Fields.CreatedOn, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.LastFiscalPeriod);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Count, 0);
        }

        [Fact]
        public void ShouldSetWhereLastFiscalYear()
        {
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereLastFiscalYear(Account.Fields.CreatedOn));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.LastFiscalYear);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Count, 0);

            var query2 = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereLastFiscalYear(Account.Fields.CreatedOn, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.LastFiscalYear);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Count, 0);
        }

        [Fact]
        public void ShouldSetWhereLastMonth()
        {
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereLastMonth(Account.Fields.CreatedOn));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.LastMonth);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Count, 0);

            var query2 = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereLastMonth(Account.Fields.CreatedOn, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.LastMonth);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Count, 0);
        }

        [Fact]
        public void ShouldSetWhereLastWeek()
        {
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereLastWeek(Account.Fields.CreatedOn));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.LastWeek);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Count, 0);

            var query2 = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereLastWeek(Account.Fields.CreatedOn, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.LastWeek);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Count, 0);
        }

        [Fact]
        public void ShouldSetWhereLastXDays()
        {
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereLastXDays(Account.Fields.CreatedOn, 10));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.LastXDays);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), 10);

            var query2 = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereLastXDays(Account.Fields.CreatedOn, 10, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.LastXDays);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), 10);
        }

        [Fact]
        public void ShouldSetWhereLastXFiscalPeriods()
        {
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereLastXFiscalPeriods(Account.Fields.CreatedOn, 10));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.LastXFiscalPeriods);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), 10);

            var query2 = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereLastXFiscalPeriods(Account.Fields.CreatedOn, 10, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.LastXFiscalPeriods);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), 10);
        }

        [Fact]
        public void ShouldSetWhereLastXFiscalYears()
        {
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereLastXFiscalYears(Account.Fields.CreatedOn, 10));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.LastXFiscalYears);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), 10);

            var query2 = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereLastXFiscalYears(Account.Fields.CreatedOn, 10, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.LastXFiscalYears);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), 10);
        }

        [Fact]
        public void ShouldSetWhereLastXHours()
        {
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereLastXHours(Account.Fields.CreatedOn, 10));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.LastXHours);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), 10);

            var query2 = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereLastXHours(Account.Fields.CreatedOn, 10, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.LastXHours);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), 10);
        }

        [Fact]
        public void ShouldSetWhereLastXMonths()
        {
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereLastXMonths(Account.Fields.CreatedOn, 10));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.LastXMonths);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), 10);

            var query2 = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereLastXMonths(Account.Fields.CreatedOn, 10, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.LastXMonths);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), 10);
        }

        [Fact]
        public void ShouldSetWhereLastXWeeks()
        {
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereLastXWeeks(Account.Fields.CreatedOn, 10));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.LastXWeeks);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), 10);

            var query2 = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereLastXWeeks(Account.Fields.CreatedOn, 10, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.LastXWeeks);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), 10);
        }

        [Fact]
        public void ShouldSetWhereLastXYears()
        {
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereLastXYears(Account.Fields.CreatedOn, 10));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.LastXYears);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), 10);

            var query2 = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereLastXYears(Account.Fields.CreatedOn, 10, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.LastXYears);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), 10);
        }

        [Fact]
        public void ShouldSetWhereLastYear()
        {
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereLastYear(Account.Fields.CreatedOn));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.LastYear);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Count, 0);

            var query2 = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereLastYear(Account.Fields.CreatedOn, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.LastYear);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Count, 0);
        }

        [Fact]
        public void ShouldSetWhereLessEqual()
        {
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereLessEqual(Account.Fields.NumberOfEmployees, 10));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.NumberOfEmployees);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.LessEqual);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), 10);

            var query2 = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereLessEqual(Account.Fields.NumberOfEmployees, 10, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.NumberOfEmployees);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.LessEqual);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), 10);
        }

        [Fact]
        public void ShouldSetWhereLessThan()
        {
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereLessThan(Account.Fields.NumberOfEmployees, 10));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.NumberOfEmployees);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.LessThan);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), 10);

            var query2 = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereLessThan(Account.Fields.NumberOfEmployees, 10, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.NumberOfEmployees);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.LessThan);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), 10);
        }

        [Fact]
        public void ShouldSetWhereLike()
        {
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereLike(Account.Fields.Name, "%test%"));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.Like);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), "%test%");

            var query2 = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereLike(Account.Fields.Name, "%test%", Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.Like);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), "%test%");
        }

        [Fact]
        public void ShouldSetWhereMask()
        {
            var obj = new object();
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereMask(Account.Fields.Name, obj));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.Mask);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), obj);

            var query2 = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereMask(Account.Fields.Name, obj, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.Mask);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), obj);
        }

        [Fact]
        public void ShouldSetWhereMasksSelect()
        {
            var obj = new object();
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereMasksSelect(Account.Fields.Name, obj));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.MasksSelect);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), obj);

            var query2 = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereMasksSelect(Account.Fields.Name, obj, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.MasksSelect);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), obj);
        }

        [Fact]
        public void ShouldSetWhereNext7Days()
        {
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNext7Days(Account.Fields.CreatedOn));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.Next7Days);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Count, 0);

            var query2 = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNext7Days(Account.Fields.CreatedOn, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.Next7Days);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Count, 0);
        }

        [Fact]
        public void ShouldSetWhereNextFiscalPeriod()
        {
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNextFiscalPeriod(Account.Fields.CreatedOn));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.NextFiscalPeriod);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Count, 0);

            var query2 = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNextFiscalPeriod(Account.Fields.CreatedOn, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.NextFiscalPeriod);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Count, 0);
        }

        [Fact]
        public void ShouldSetWhereNextFiscalYear()
        {
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNextFiscalYear(Account.Fields.CreatedOn));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.NextFiscalYear);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Count, 0);

            var query2 = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNextFiscalYear(Account.Fields.CreatedOn, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.NextFiscalYear);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Count, 0);
        }

        [Fact]
        public void ShouldSetWhereNextMonth()
        {
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNextMonth(Account.Fields.CreatedOn));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.NextMonth);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Count, 0);

            var query2 = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNextMonth(Account.Fields.CreatedOn, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.NextMonth);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Count, 0);
        }

        [Fact]
        public void ShouldSetWhereNextWeek()
        {
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNextWeek(Account.Fields.CreatedOn));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.NextWeek);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Count, 0);

            var query2 = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNextWeek(Account.Fields.CreatedOn, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.NextWeek);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Count, 0);
        }

        [Fact]
        public void ShouldSetWhereNextXDays()
        {
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNextXDays(Account.Fields.CreatedOn, 10));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.NextXDays);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), 10);

            var query2 = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNextXDays(Account.Fields.CreatedOn, 10, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.NextXDays);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), 10);
        }

        [Fact]
        public void ShouldSetWhereNextXFiscalPeriods()
        {
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNextXFiscalPeriods(Account.Fields.CreatedOn, 10));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.NextXFiscalPeriods);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), 10);

            var query2 = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNextXFiscalPeriods(Account.Fields.CreatedOn, 10, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.NextXFiscalPeriods);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), 10);
        }

        [Fact]
        public void ShouldSetWhereNextXFiscalYears()
        {
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNextXFiscalYears(Account.Fields.CreatedOn, 10));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.NextXFiscalYears);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), 10);

            var query2 = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNextXFiscalYears(Account.Fields.CreatedOn, 10, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.NextXFiscalYears);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), 10);
        }

        [Fact]
        public void ShouldSetWhereNextXHours()
        {
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNextXHours(Account.Fields.CreatedOn, 10));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.NextXHours);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), 10);

            var query2 = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNextXHours(Account.Fields.CreatedOn, 10, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.NextXHours);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), 10);
        }

        [Fact]
        public void ShouldSetWhereNextXMonths()
        {
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNextXMonths(Account.Fields.CreatedOn, 10));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.NextXMonths);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), 10);

            var query2 = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNextXMonths(Account.Fields.CreatedOn, 10, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.NextXMonths);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), 10);
        }

        [Fact]
        public void ShouldSetWhereNextXWeeks()
        {
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNextXWeeks(Account.Fields.CreatedOn, 10));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.NextXWeeks);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), 10);

            var query2 = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNextXWeeks(Account.Fields.CreatedOn, 10, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.NextXWeeks);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), 10);
        }

        [Fact]
        public void ShouldSetWhereNextXYears()
        {
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNextXYears(Account.Fields.CreatedOn, 10));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.NextXYears);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), 10);

            var query2 = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNextXYears(Account.Fields.CreatedOn, 10, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.NextXYears);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), 10);
        }

        [Fact]
        public void ShouldSetWhereNextYear()
        {
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNextYear(Account.Fields.CreatedOn));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.NextYear);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Count, 0);

            var query2 = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNextYear(Account.Fields.CreatedOn, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.NextYear);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Count, 0);
        }

        [Fact]
        public void ShouldSetWhereNotBetween()
        {
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNotBetween(Account.Fields.NumberOfEmployees, 10, 50));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.NumberOfEmployees);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.NotBetween);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values[0], 10);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values[1], 50);

            var query2 = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNotBetween(Account.Fields.NumberOfEmployees, 10, 50, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.NumberOfEmployees);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.NotBetween);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values[0], 10);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values[1], 50);
        }

        [Fact]
        public void ShouldSetWhereNotEqual()
        {
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNotEqual(Account.Fields.Name, "test"));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.NotEqual);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), "test");

            var query2 = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNotEqual(Account.Fields.Name, "test", Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.NotEqual);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), "test");
        }

        [Fact]
        public void ShouldSetWhereNotEqualBusinessId()
        {
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNotEqualBusinessId(Account.Fields.OwningBusinessUnit));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.OwningBusinessUnit);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.NotEqualBusinessId);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Count, 0);

            var query2 = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNotEqualBusinessId(Account.Fields.OwningBusinessUnit, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.OwningBusinessUnit);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.NotEqualBusinessId);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Count, 0);
        }

        [Fact]
        public void ShouldSetWhereNotEqualUserId()
        {
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNotEqualUserId(Account.Fields.OwnerId));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.OwnerId);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.NotEqualUserId);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Count, 0);

            var query2 = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNotEqualUserId(Account.Fields.OwnerId, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.OwnerId);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.NotEqualUserId);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Count, 0);
        }

        [Fact]
        public void ShouldSetWhereNotIn()
        {
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNotIn(Account.Fields.CustomerTypeCode, 1, 2, 3));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CustomerTypeCode);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.NotIn);
            Assert.True(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Contains(1));
            Assert.True(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Contains(2));
            Assert.True(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Contains(3));

            var query2 = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNotIn(Account.EntityLogicalName, Account.Fields.CustomerTypeCode, 1, 2, 3));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CustomerTypeCode);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.NotIn);
            Assert.True(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Contains(1));
            Assert.True(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Contains(2));
            Assert.True(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Contains(3));
        }

        [Fact]
        public void ShouldSetWhereNotInList()
        {
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId)
                    .WhereNotIn(Account.Fields.CustomerTypeCode, new List<int> { 1, 2, 3 }));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CustomerTypeCode);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.NotIn);
            Assert.True(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values is IList);

            var query2 = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId)
                    .WhereNotIn(Account.EntityLogicalName, Account.Fields.CustomerTypeCode, new List<int> { 1, 2, 3 }));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CustomerTypeCode);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.NotIn);
            Assert.True(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values is IList);
        }

        [Fact]
        public void ShouldSetWhereNotLike()
        {
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNotLike(Account.Fields.Name, "%test%"));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.NotLike);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), "%test%");

            var query2 = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNotLike(Account.Fields.Name, "%test%", Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.NotLike);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), "%test%");
        }

        [Fact]
        public void ShouldSetWhereNotMask()
        {
            var obj = new object();
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNotMask(Account.Fields.Name, obj));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.NotMask);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), obj);

            var query2 = new Query<Account>()
                 .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNotMask(Account.Fields.Name, obj, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.NotMask);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), obj);
        }

        [Fact]
        public void ShouldSetWhereNotNull()
        {
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNotNull(Account.Fields.Name));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.NotNull);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Count, 0);

            var query2 = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNotNull(Account.Fields.Name, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.NotNull);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Count, 0);
        }

        [Fact]
        public void ShouldSetWhereNotOn()
        {
            var date = new DateTime();
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNotOn(Account.Fields.CreatedOn, date));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.NotOn);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), date);

            var query2 = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNotOn(Account.Fields.CreatedOn, date, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.NotOn);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), date);
        }

        [Fact]
        public void ShouldSetWhereNotUnder()
        {
            var guid = new Guid();
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNotUnder(Account.Fields.AccountId, guid));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.AccountId);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.NotUnder);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), guid);

            var query2 = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNotUnder(Account.Fields.AccountId, guid, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.AccountId);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.NotUnder);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), guid);
        }

        [Fact]
        public void ShouldSetWhereNull()
        {
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNull(Account.Fields.Name));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.Null);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Count, 0);

            var query2 = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereNull(Account.Fields.Name, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.Name);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.Null);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Count, 0);
        }

        [Fact]
        public void ShouldSetWhereOlderThanXDays()
        {
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereOlderThanXDays(Account.Fields.CreatedOn, 10));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.OlderThanXDays);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), 10);

            var query2 = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereOlderThanXDays(Account.Fields.CreatedOn, 10, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.OlderThanXDays);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), 10);
        }

        [Fact]
        public void ShouldSetWhereOlderThanXHours()
        {
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereOlderThanXHours(Account.Fields.CreatedOn, 10));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.OlderThanXHours);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), 10);

            var query2 = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereOlderThanXHours(Account.Fields.CreatedOn, 10, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.OlderThanXHours);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), 10);
        }

        [Fact]
        public void ShouldSetWhereOlderThanXMinutes()
        {
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereOlderThanXMinutes(Account.Fields.CreatedOn, 10));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.OlderThanXMinutes);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), 10);

            var query2 = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereOlderThanXMinutes(Account.Fields.CreatedOn, 10, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.OlderThanXMinutes);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), 10);
        }

        [Fact]
        public void ShouldSetWhereOlderThanXMonths()
        {
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereOlderThanXMonths(Account.Fields.CreatedOn, 10));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.OlderThanXMonths);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), 10);

            var query2 = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereOlderThanXMonths(Account.Fields.CreatedOn, 10, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.OlderThanXMonths);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), 10);
        }

        [Fact]
        public void ShouldSetWhereOlderThanXWeeks()
        {
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereOlderThanXWeeks(Account.Fields.CreatedOn, 10));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.OlderThanXWeeks);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), 10);

            var query2 = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereOlderThanXWeeks(Account.Fields.CreatedOn, 10, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.OlderThanXWeeks);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), 10);
        }

        [Fact]
        public void ShouldSetWhereOlderThanXYears()
        {
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereOlderThanXYears(Account.Fields.CreatedOn, 10));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.OlderThanXYears);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), 10);

            var query2 = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereOlderThanXYears(Account.Fields.CreatedOn, 10, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.OlderThanXYears);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), 10);
        }

        [Fact]
        public void ShouldSetWhereOn()
        {
            var date = new DateTime();
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereOn(Account.Fields.CreatedOn, date));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.On);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), date);

            var query2 = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereOn(Account.Fields.CreatedOn, date, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.On);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), date);
        }

        [Fact]
        public void ShouldSetWhereOnOrAfter()
        {
            var date = new DateTime();
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereOnOrAfter(Account.Fields.CreatedOn, date));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.OnOrAfter);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), date);

            var query2 = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereOnOrAfter(Account.Fields.CreatedOn, date, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.OnOrAfter);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), date);
        }

        [Fact]
        public void ShouldSetWhereOnOrBefore()
        {
            var date = new DateTime();
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereOnOrBefore(Account.Fields.CreatedOn, date));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.OnOrBefore);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), date);

            var query2 = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereOnOrBefore(Account.Fields.CreatedOn, date, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.OnOrBefore);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), date);
        }

        [Fact]
        public void ShouldSetWhereThisFiscalPeriod()
        {
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereThisFiscalPeriod(Account.Fields.CreatedOn));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.ThisFiscalPeriod);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Count, 0);

            var query2 = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereThisFiscalPeriod(Account.Fields.CreatedOn, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.ThisFiscalPeriod);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Count, 0);
        }

        [Fact]
        public void ShouldSetWhereThisFiscalYear()
        {
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereThisFiscalYear(Account.Fields.CreatedOn));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.ThisFiscalYear);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Count, 0);

            var query2 = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereThisFiscalYear(Account.Fields.CreatedOn, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.ThisFiscalYear);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Count, 0);
        }

        [Fact]
        public void ShouldSetWhereThisMonth()
        {
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereThisMonth(Account.Fields.CreatedOn));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.ThisMonth);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Count, 0);

            var query2 = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereThisMonth(Account.Fields.CreatedOn, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.ThisMonth);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Count, 0);
        }

        [Fact]
        public void ShouldSetWhereThisWeek()
        {
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereThisWeek(Account.Fields.CreatedOn));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.ThisWeek);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Count, 0);

            var query2 = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereThisWeek(Account.Fields.CreatedOn, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.ThisWeek);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Count, 0);
        }

        [Fact]
        public void ShouldSetWhereThisYear()
        {
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereThisYear(Account.Fields.CreatedOn));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.ThisYear);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Count, 0);

            var query2 = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereThisYear(Account.Fields.CreatedOn, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.ThisYear);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Count, 0);
        }

        [Fact]
        public void ShouldSetWhereToday()
        {
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereToday(Account.Fields.CreatedOn));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.Today);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Count, 0);

            var query2 = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereToday(Account.Fields.CreatedOn, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.Today);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Count, 0);
        }

        [Fact]
        public void ShouldSetWhereTomorrow()
        {
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereTomorrow(Account.Fields.CreatedOn));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.Tomorrow);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Count, 0);

            var query2 = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereTomorrow(Account.Fields.CreatedOn, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.Tomorrow);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Count, 0);
        }

        [Fact]
        public void ShouldSetWhereUnder()
        {
            Guid guid = new Guid();
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereUnder(Account.Fields.AccountId, guid));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.AccountId);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.Under);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), guid);

            var query2 = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereUnder(Account.Fields.AccountId, guid, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.AccountId);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.Under);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), guid);
        }

        [Fact]
        public void ShouldSetWhereUnderOrEqual()
        {
            Guid guid = new Guid();
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereUnderOrEqual(Account.Fields.AccountId, guid));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.AccountId);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.UnderOrEqual);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), guid);

            var query2 = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereUnderOrEqual(Account.Fields.AccountId, guid, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.AccountId);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.UnderOrEqual);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.First(), guid);
        }

        [Fact]
        public void ShouldSetWhereYesterday()
        {
            var query = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereYesterday(Account.Fields.CreatedOn));

            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.Yesterday);
            Assert.Equal(query.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Count, 0);

            var query2 = new Query<Account>()
                .AddLink(new Link<Contact>(Contact.Fields.ParentCustomerId, Account.Fields.AccountId).WhereYesterday(Account.Fields.CreatedOn, Account.EntityLogicalName));

            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().EntityName, Account.EntityLogicalName);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().AttributeName, Account.Fields.CreatedOn);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Operator, ConditionOperator.Yesterday);
            Assert.Equal(query2.QueryExpression.LinkEntities.First().LinkCriteria.Conditions.First().Values.Count, 0);
        }

        #endregion Conditions
    }
}