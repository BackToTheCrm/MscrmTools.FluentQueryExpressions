using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections;
using System.Linq;

namespace MscrmTools.FluentQueryExpressions
{
    public class Link : Link<Entity>
    {
        public Link(string toEntity, string toAttribute, string fromAttribute, JoinOperator joinOperator = JoinOperator.Inner) :
            base(toEntity, toAttribute, fromAttribute, joinOperator)
        {
            ToEntity = toEntity;
        }
    }

    public class Link<TEntity> where TEntity : Entity
    {
        protected string ToEntity;

        public Link(string toAttribute, string fromAttribute, JoinOperator joinOperator = JoinOperator.Inner)
        {
            var toEntity = typeof(TEntity).GetField("EntityLogicalName").GetRawConstantValue().ToString();
            ToEntity = toEntity;
            InnerLinkEntity = new LinkEntity(null, toEntity, fromAttribute, toAttribute, joinOperator)
            { EntityAlias = toEntity };
        }

        protected Link(string toEntity, string toAttribute, string fromAttribute, JoinOperator joinOperator = JoinOperator.Inner)
        {
            InnerLinkEntity = new LinkEntity(null, toEntity, fromAttribute, toAttribute, joinOperator)
            { EntityAlias = toEntity };
        }

        public LinkEntity InnerLinkEntity { get; }

        public Link<TEntity> AddFilters(params Filter[] filters)
        {
            InnerLinkEntity.LinkCriteria.Filters.AddRange(filters.Select(f => f.InnerFilter));

            return this;
        }

        public Link<TEntity> AddFilters(LogicalOperator logicalOperator, params Filter[] filters)
        {
            InnerLinkEntity.LinkCriteria.FilterOperator = logicalOperator;
            InnerLinkEntity.LinkCriteria.Filters.AddRange(filters.Select(f => f.InnerFilter));

            return this;
        }

        public Link<TEntity> AddLink<TU>(Link<TU> link) where TU : Entity
        {
            var fromEntity = typeof(TU) == typeof(Entity) ? ToEntity : typeof(TU).GetField("EntityLogicalName").GetRawConstantValue().ToString();

            InnerLinkEntity.LinkEntities.Add(link.InnerLinkEntity);

            link.InnerLinkEntity.LinkFromEntityName = fromEntity;

            return this;
        }

        public Link<TEntity> Select(bool allColumns)
        {
            InnerLinkEntity.Columns = new ColumnSet(allColumns);

            return this;
        }

        public Link<TEntity> Select(params string[] attributes)
        {
            InnerLinkEntity.Columns.AddColumns(attributes);

            return this;
        }

        public Link<TEntity> SetAlias(string alias)
        {
            InnerLinkEntity.EntityAlias = alias;

            return this;
        }

        public Link<TEntity> SetDefaultFilterOperator(LogicalOperator logicalOperator)
        {
            InnerLinkEntity.LinkCriteria.FilterOperator = logicalOperator;

            return this;
        }

        public Link<TEntity> Order(string attribute, OrderType order)
        {
            InnerLinkEntity.Orders.Add(new OrderExpression(attribute, order));

            return this;
        }

        #region Conditions

        public Link<TEntity> Where(string attributeName, ConditionOperator conditionOperator, params object[] values)
        {
            InnerLinkEntity.LinkCriteria.AddCondition(attributeName, conditionOperator, values);

            return this;
        }

        public Link<TEntity> Where(string entityName, string attributeName, ConditionOperator conditionOperator, params object[] values)
        {
            InnerLinkEntity.LinkCriteria.AddCondition(entityName, attributeName, conditionOperator, values);

            return this;
        }

        public Link<TEntity> WhereAbove(string attributeName, object value, string entityname = null)
        {
            if (entityname != null)
            {
                InnerLinkEntity.LinkCriteria.AddCondition(entityname, attributeName, ConditionOperator.Above, value);
            }
            else
            {
                InnerLinkEntity.LinkCriteria.AddCondition(attributeName, ConditionOperator.Above, value);
            }

            return this;
        }

        public Link<TEntity> WhereAboveOrEqual(string attributeName, object value, string entityname = null)
        {
            if (entityname != null)
            {
                InnerLinkEntity.LinkCriteria.AddCondition(entityname, attributeName, ConditionOperator.AboveOrEqual, value);
            }
            else
            {
                InnerLinkEntity.LinkCriteria.AddCondition(attributeName, ConditionOperator.AboveOrEqual, value);
            }

            return this;
        }

        public Link<TEntity> WhereBeginsWith(string attributeName, object value, string entityname = null)
        {
            if (entityname != null)
            {
                InnerLinkEntity.LinkCriteria.AddCondition(entityname, attributeName, ConditionOperator.BeginsWith, value);
            }
            else
            {
                InnerLinkEntity.LinkCriteria.AddCondition(attributeName, ConditionOperator.BeginsWith, value);
            }

            return this;
        }

        public Link<TEntity> WhereBetween(string attributeName, object value1, object value2, string entityname = null)
        {
            if (entityname != null)
            {
                InnerLinkEntity.LinkCriteria.AddCondition(entityname, attributeName, ConditionOperator.Between, value1, value2);
            }
            else
            {
                InnerLinkEntity.LinkCriteria.AddCondition(attributeName, ConditionOperator.Between, value1, value2);
            }

            return this;
        }

        public Link<TEntity> WhereChildOf(string attributeName, object value, string entityname = null)
        {
            if (entityname != null)
            {
                InnerLinkEntity.LinkCriteria.AddCondition(entityname, attributeName, ConditionOperator.ChildOf, value);
            }
            else
            {
                InnerLinkEntity.LinkCriteria.AddCondition(attributeName, ConditionOperator.ChildOf, value);
            }

            return this;
        }

        public Link<TEntity> WhereContains(string attributeName, object value, string entityname = null)
        {
            if (entityname != null)
            {
                InnerLinkEntity.LinkCriteria.AddCondition(entityname, attributeName, ConditionOperator.Contains, value);
            }
            else
            {
                InnerLinkEntity.LinkCriteria.AddCondition(attributeName, ConditionOperator.Contains, value);
            }

            return this;
        }
#if CRMV9
        public Link<TEntity> WhereContainValues(string attributeName, params object[] values)
        {
            InnerLinkEntity.LinkCriteria.AddCondition(attributeName, ConditionOperator.ContainValues, values);

            return this;
        }

        public Link<TEntity> WhereContainValues(string entityname, string attributeName, params object[] values)
        {
            InnerLinkEntity.LinkCriteria.AddCondition(entityname, attributeName, ConditionOperator.ContainValues, values);

            return this;
        }
#endif
        public Link<TEntity> WhereDoesNotBeginWith(string attributeName, object value, string entityname = null)
        {
            if (entityname != null)
            {
                InnerLinkEntity.LinkCriteria.AddCondition(entityname, attributeName, ConditionOperator.DoesNotBeginWith, value);
            }
            else
            {
                InnerLinkEntity.LinkCriteria.AddCondition(attributeName, ConditionOperator.DoesNotBeginWith, value);
            }

            return this;
        }

        public Link<TEntity> WhereDoesNotContain(string attributeName, object value, string entityname = null)
        {
            if (entityname != null)
            {
                InnerLinkEntity.LinkCriteria.AddCondition(entityname, attributeName, ConditionOperator.DoesNotContain, value);
            }
            else
            {
                InnerLinkEntity.LinkCriteria.AddCondition(attributeName, ConditionOperator.DoesNotContain, value);
            }

            return this;
        }
#if CRMV9
        public Link<TEntity> WhereDoesNotContainValues(string attributeName, params object[] values)
        {
            InnerLinkEntity.LinkCriteria.AddCondition(attributeName, ConditionOperator.DoesNotContainValues, values);

            return this;
        }

        public Link<TEntity> WhereDoesNotContainValues(string entityname, string attributeName, params object[] values)
        {
            InnerLinkEntity.LinkCriteria.AddCondition(entityname, attributeName, ConditionOperator.DoesNotContainValues, values);

            return this;
        }
#endif

        public Link<TEntity> WhereDoesNotEndWith(string attributeName, object value, string entityname = null)
        {
            if (entityname != null)
            {
                InnerLinkEntity.LinkCriteria.AddCondition(entityname, attributeName, ConditionOperator.DoesNotEndWith, value);
            }
            else
            {
                InnerLinkEntity.LinkCriteria.AddCondition(attributeName, ConditionOperator.DoesNotEndWith, value);
            }

            return this;
        }

        public Link<TEntity> WhereEndsWith(string attributeName, object value, string entityname = null)
        {
            if (entityname != null)
            {
                InnerLinkEntity.LinkCriteria.AddCondition(entityname, attributeName, ConditionOperator.EndsWith, value);
            }
            else
            {
                InnerLinkEntity.LinkCriteria.AddCondition(attributeName, ConditionOperator.EndsWith, value);
            }

            return this;
        }

        public Link<TEntity> WhereEqual(string attributeName, object value, string entityname = null)
        {
            if (entityname != null)
            {
                InnerLinkEntity.LinkCriteria.AddCondition(entityname, attributeName, ConditionOperator.Equal, value);
            }
            else
            {
                InnerLinkEntity.LinkCriteria.AddCondition(attributeName, ConditionOperator.Equal, value);
            }

            return this;
        }

        public Link<TEntity> WhereEqualBusinessId(string attributeName, string entityname = null)
        {
            if (entityname != null)
            {
                InnerLinkEntity.LinkCriteria.AddCondition(entityname, attributeName, ConditionOperator.EqualBusinessId);
            }
            else
            {
                InnerLinkEntity.LinkCriteria.AddCondition(attributeName, ConditionOperator.EqualBusinessId);
            }

            return this;
        }

        public Link<TEntity> WhereEqualUserId(string attributeName, string entityname = null)
        {
            if (entityname != null)
            {
                InnerLinkEntity.LinkCriteria.AddCondition(entityname, attributeName, ConditionOperator.EqualUserId);
            }
            else
            {
                InnerLinkEntity.LinkCriteria.AddCondition(attributeName, ConditionOperator.EqualUserId);
            }

            return this;
        }

        public Link<TEntity> WhereEqualUserLanguage(string attributeName, string entityname = null)
        {
            if (entityname != null)
            {
                InnerLinkEntity.LinkCriteria.AddCondition(entityname, attributeName, ConditionOperator.EqualUserLanguage);
            }
            else
            {
                InnerLinkEntity.LinkCriteria.AddCondition(attributeName, ConditionOperator.EqualUserLanguage);
            }

            return this;
        }

        public Link<TEntity> WhereEqualUserOrUserHierarchy(string attributeName, string entityname = null)
        {
            if (entityname != null)
            {
                InnerLinkEntity.LinkCriteria.AddCondition(entityname, attributeName, ConditionOperator.EqualUserOrUserHierarchy);
            }
            else
            {
                InnerLinkEntity.LinkCriteria.AddCondition(attributeName, ConditionOperator.EqualUserOrUserHierarchy);
            }

            return this;
        }

        public Link<TEntity> WhereEqualUserOrUserHierarchyAndTeams(string attributeName, string entityname = null)
        {
            if (entityname != null)
            {
                InnerLinkEntity.LinkCriteria.AddCondition(entityname, attributeName, ConditionOperator.EqualUserOrUserHierarchyAndTeams);
            }
            else
            {
                InnerLinkEntity.LinkCriteria.AddCondition(attributeName,
                    ConditionOperator.EqualUserOrUserHierarchyAndTeams);
            }

            return this;
        }

        public Link<TEntity> WhereEqualUserOrUserTeams(string attributeName, string entityname = null)
        {
            if (entityname != null)
            {
                InnerLinkEntity.LinkCriteria.AddCondition(entityname, attributeName, ConditionOperator.EqualUserOrUserTeams);
            }
            else
            {
                InnerLinkEntity.LinkCriteria.AddCondition(attributeName, ConditionOperator.EqualUserOrUserTeams);
            }

            return this;
        }

        public Link<TEntity> WhereEqualUserTeams(string attributeName, string entityname = null)
        {
            if (entityname != null)
            {
                InnerLinkEntity.LinkCriteria.AddCondition(entityname, attributeName, ConditionOperator.EqualUserTeams);
            }
            else
            {
                InnerLinkEntity.LinkCriteria.AddCondition(attributeName, ConditionOperator.EqualUserTeams);
            }

            return this;
        }

        public Link<TEntity> WhereGreaterEqual(string attributeName, object value, string entityname = null)
        {
            if (entityname != null)
            {
                InnerLinkEntity.LinkCriteria.AddCondition(entityname, attributeName, ConditionOperator.GreaterEqual, value);
            }
            else
            {
                InnerLinkEntity.LinkCriteria.AddCondition(attributeName, ConditionOperator.GreaterEqual, value);
            }

            return this;
        }

        public Link<TEntity> WhereGreaterThan(string attributeName, object value, string entityname = null)
        {
            if (entityname != null)
            {
                InnerLinkEntity.LinkCriteria.AddCondition(entityname, attributeName, ConditionOperator.GreaterThan, value);
            }
            else
            {
                InnerLinkEntity.LinkCriteria.AddCondition(attributeName, ConditionOperator.GreaterThan, value);
            }

            return this;
        }

        public Link<TEntity> WhereIn(string attributeName, params object[] values)
        {
            InnerLinkEntity.LinkCriteria.AddCondition(attributeName, ConditionOperator.In, values);

            return this;
        }

        public Link<TEntity> WhereIn(string entityname, string attributeName, params object[] values)
        {
            InnerLinkEntity.LinkCriteria.AddCondition(entityname, attributeName, ConditionOperator.In, values);

            return this;
        }

        public Link<TEntity> WhereIn(string attributeName, IList value)
        {
            InnerLinkEntity.LinkCriteria.Conditions.Add(new ConditionExpression(attributeName, ConditionOperator.In, value));

            return this;
        }

        public Link<TEntity> WhereIn(string entityname, string attributeName, IList value)
        {
            InnerLinkEntity.LinkCriteria.Conditions.Add(new ConditionExpression(entityname, attributeName, ConditionOperator.In, value));

            return this;
        }

        public Link<TEntity> WhereInFiscalPeriod(string attributeName, int period, string entityname = null)
        {
            if (entityname != null)
            {
                InnerLinkEntity.LinkCriteria.AddCondition(entityname, attributeName, ConditionOperator.InFiscalPeriod, period);
            }
            else
            {
                InnerLinkEntity.LinkCriteria.AddCondition(attributeName, ConditionOperator.InFiscalPeriod, period);
            }

            return this;
        }

        public Link<TEntity> WhereInFiscalPeriodAndYear(string attributeName, int period, int year, string entityname = null)
        {
            if (entityname != null)
            {
                InnerLinkEntity.LinkCriteria.AddCondition(entityname, attributeName, ConditionOperator.InFiscalPeriodAndYear, period, year);
            }
            else
            {
                InnerLinkEntity.LinkCriteria.AddCondition(attributeName, ConditionOperator.InFiscalPeriodAndYear, period, year);
            }

            return this;
        }

        public Link<TEntity> WhereInFiscalYear(string attributeName, int year, string entityname = null)
        {
            if (entityname != null)
            {
                InnerLinkEntity.LinkCriteria.AddCondition(entityname, attributeName, ConditionOperator.InFiscalYear, year);
            }
            else
            {
                InnerLinkEntity.LinkCriteria.AddCondition(attributeName, ConditionOperator.InFiscalYear, year);
            }

            return this;
        }

        public Link<TEntity> WhereInOrAfterFiscalPeriodAndYear(string attributeName, int period, int year, string entityname = null)
        {
            if (entityname != null)
            {
                InnerLinkEntity.LinkCriteria.AddCondition(entityname, attributeName, ConditionOperator.InOrAfterFiscalPeriodAndYear, period, year);
            }
            else
            {
                InnerLinkEntity.LinkCriteria.AddCondition(attributeName, ConditionOperator.InOrAfterFiscalPeriodAndYear,
                    period, year);
            }

            return this;
        }

        public Link<TEntity> WhereInOrBeforeFiscalPeriodAndYear(string attributeName, int period, int year, string entityname = null)
        {
            if (entityname != null)
            {
                InnerLinkEntity.LinkCriteria.AddCondition(entityname, attributeName, ConditionOperator.InOrBeforeFiscalPeriodAndYear, period, year);
            }
            else
            {
                InnerLinkEntity.LinkCriteria.AddCondition(attributeName, ConditionOperator.InOrBeforeFiscalPeriodAndYear,
                    period, year);
            }

            return this;
        }

        public Link<TEntity> WhereLast7Days(string attributeName, string entityname = null)
        {
            if (entityname != null)
            {
                InnerLinkEntity.LinkCriteria.AddCondition(entityname, attributeName, ConditionOperator.Last7Days);
            }
            else
            {
                InnerLinkEntity.LinkCriteria.AddCondition(attributeName, ConditionOperator.Last7Days);
            }

            return this;
        }

        public Link<TEntity> WhereLastFiscalPeriod(string attributeName, string entityname = null)
        {
            if (entityname != null)
            {
                InnerLinkEntity.LinkCriteria.AddCondition(entityname, attributeName, ConditionOperator.LastFiscalPeriod);
            }
            else
            {
                InnerLinkEntity.LinkCriteria.AddCondition(attributeName, ConditionOperator.LastFiscalPeriod);
            }

            return this;
        }

        public Link<TEntity> WhereLastFiscalYear(string attributeName, string entityname = null)
        {
            if (entityname != null)
            {
                InnerLinkEntity.LinkCriteria.AddCondition(entityname, attributeName, ConditionOperator.LastFiscalYear);
            }
            else
            {
                InnerLinkEntity.LinkCriteria.AddCondition(attributeName, ConditionOperator.LastFiscalYear);
            }

            return this;
        }

        public Link<TEntity> WhereLastMonth(string attributeName, string entityname = null)
        {
            if (entityname != null)
            {
                InnerLinkEntity.LinkCriteria.AddCondition(entityname, attributeName, ConditionOperator.LastMonth);
            }
            else
            {
                InnerLinkEntity.LinkCriteria.AddCondition(attributeName, ConditionOperator.LastMonth);
            }

            return this;
        }

        public Link<TEntity> WhereLastWeek(string attributeName, string entityname = null)
        {
            if (entityname != null)
            {
                InnerLinkEntity.LinkCriteria.AddCondition(entityname, attributeName, ConditionOperator.LastWeek);
            }
            else
            {
                InnerLinkEntity.LinkCriteria.AddCondition(attributeName, ConditionOperator.LastWeek);
            }

            return this;
        }

        public Link<TEntity> WhereLastXDays(string attributeName, int value, string entityname = null)
        {
            if (entityname != null)
            {
                InnerLinkEntity.LinkCriteria.AddCondition(entityname, attributeName, ConditionOperator.LastXDays, value);
            }
            else
            {
                InnerLinkEntity.LinkCriteria.AddCondition(attributeName, ConditionOperator.LastXDays, value);
            }

            return this;
        }

        public Link<TEntity> WhereLastXFiscalPeriods(string attributeName, int value, string entityname = null)
        {
            if (entityname != null)
            {
                InnerLinkEntity.LinkCriteria.AddCondition(entityname, attributeName, ConditionOperator.LastXFiscalPeriods, value);
            }
            else
            {
                InnerLinkEntity.LinkCriteria.AddCondition(attributeName, ConditionOperator.LastXFiscalPeriods, value);
            }

            return this;
        }

        public Link<TEntity> WhereLastXFiscalYears(string attributeName, int value, string entityname = null)
        {
            if (entityname != null)
            {
                InnerLinkEntity.LinkCriteria.AddCondition(entityname, attributeName, ConditionOperator.LastXFiscalYears, value);
            }
            else
            {
                InnerLinkEntity.LinkCriteria.AddCondition(attributeName, ConditionOperator.LastXFiscalYears, value);
            }

            return this;
        }

        public Link<TEntity> WhereLastXHours(string attributeName, int value, string entityname = null)
        {
            if (entityname != null)
            {
                InnerLinkEntity.LinkCriteria.AddCondition(entityname, attributeName, ConditionOperator.LastXHours, value);
            }
            else
            {
                InnerLinkEntity.LinkCriteria.AddCondition(attributeName, ConditionOperator.LastXHours, value);
            }

            return this;
        }

        public Link<TEntity> WhereLastXMonths(string attributeName, int value, string entityname = null)
        {
            if (entityname != null)
            {
                InnerLinkEntity.LinkCriteria.AddCondition(entityname, attributeName, ConditionOperator.LastXMonths, value);
            }
            else
            {
                InnerLinkEntity.LinkCriteria.AddCondition(attributeName, ConditionOperator.LastXMonths, value);
            }

            return this;
        }

        public Link<TEntity> WhereLastXWeeks(string attributeName, int value, string entityname = null)
        {
            if (entityname != null)
            {
                InnerLinkEntity.LinkCriteria.AddCondition(entityname, attributeName, ConditionOperator.LastXWeeks, value);
            }
            else
            {
                InnerLinkEntity.LinkCriteria.AddCondition(attributeName, ConditionOperator.LastXWeeks, value);
            }

            return this;
        }

        public Link<TEntity> WhereLastXYears(string attributeName, int value, string entityname = null)
        {
            if (entityname != null)
            {
                InnerLinkEntity.LinkCriteria.AddCondition(entityname, attributeName, ConditionOperator.LastXYears, value);
            }
            else
            {
                InnerLinkEntity.LinkCriteria.AddCondition(attributeName, ConditionOperator.LastXYears, value);
            }

            return this;
        }

        public Link<TEntity> WhereLastYear(string attributeName, string entityname = null)
        {
            if (entityname != null)
            {
                InnerLinkEntity.LinkCriteria.AddCondition(entityname, attributeName, ConditionOperator.LastYear);
            }
            else
            {
                InnerLinkEntity.LinkCriteria.AddCondition(attributeName, ConditionOperator.LastYear);
            }

            return this;
        }

        public Link<TEntity> WhereLessEqual(string attributeName, object value, string entityname = null)
        {
            if (entityname != null)
            {
                InnerLinkEntity.LinkCriteria.AddCondition(entityname, attributeName, ConditionOperator.LessEqual, value);
            }
            else
            {
                InnerLinkEntity.LinkCriteria.AddCondition(attributeName, ConditionOperator.LessEqual, value);
            }

            return this;
        }

        public Link<TEntity> WhereLessThan(string attributeName, object value, string entityname = null)
        {
            if (entityname != null)
            {
                InnerLinkEntity.LinkCriteria.AddCondition(entityname, attributeName, ConditionOperator.LessThan, value);
            }
            else
            {
                InnerLinkEntity.LinkCriteria.AddCondition(attributeName, ConditionOperator.LessThan, value);
            }

            return this;
        }

        public Link<TEntity> WhereLike(string attributeName, object value, string entityname = null)
        {
            if (entityname != null)
            {
                InnerLinkEntity.LinkCriteria.AddCondition(entityname, attributeName, ConditionOperator.Like, value);
            }
            else
            {
                InnerLinkEntity.LinkCriteria.AddCondition(attributeName, ConditionOperator.Like, value);
            }

            return this;
        }

        public Link<TEntity> WhereMask(string attributeName, object value, string entityname = null)
        {
            if (entityname != null)
            {
                InnerLinkEntity.LinkCriteria.AddCondition(entityname, attributeName, ConditionOperator.Mask, value);
            }
            else
            {
                InnerLinkEntity.LinkCriteria.AddCondition(attributeName, ConditionOperator.Mask, value);
            }

            return this;
        }

        public Link<TEntity> WhereMasksSelect(string attributeName, object value, string entityname = null)
        {
            if (entityname != null)
            {
                InnerLinkEntity.LinkCriteria.AddCondition(entityname, attributeName, ConditionOperator.MasksSelect, value);
            }
            else
            {
                InnerLinkEntity.LinkCriteria.AddCondition(attributeName, ConditionOperator.MasksSelect, value);
            }

            return this;
        }

        public Link<TEntity> WhereNext7Days(string attributeName, string entityname = null)
        {
            if (entityname != null)
            {
                InnerLinkEntity.LinkCriteria.AddCondition(entityname, attributeName, ConditionOperator.Next7Days);
            }
            else
            {
                InnerLinkEntity.LinkCriteria.AddCondition(attributeName, ConditionOperator.Next7Days);
            }

            return this;
        }

        public Link<TEntity> WhereNextFiscalPeriod(string attributeName, string entityname = null)
        {
            if (entityname != null)
            {
                InnerLinkEntity.LinkCriteria.AddCondition(entityname, attributeName, ConditionOperator.NextFiscalPeriod);
            }
            else
            {
                InnerLinkEntity.LinkCriteria.AddCondition(attributeName, ConditionOperator.NextFiscalPeriod);
            }

            return this;
        }

        public Link<TEntity> WhereNextFiscalYear(string attributeName, string entityname = null)
        {
            if (entityname != null)
            {
                InnerLinkEntity.LinkCriteria.AddCondition(entityname, attributeName, ConditionOperator.NextFiscalYear);
            }
            else
            {
                InnerLinkEntity.LinkCriteria.AddCondition(attributeName, ConditionOperator.NextFiscalYear);
            }

            return this;
        }

        public Link<TEntity> WhereNextMonth(string attributeName, string entityname = null)
        {
            if (entityname != null)
            {
                InnerLinkEntity.LinkCriteria.AddCondition(entityname, attributeName, ConditionOperator.NextMonth);
            }
            else
            {
                InnerLinkEntity.LinkCriteria.AddCondition(attributeName, ConditionOperator.NextMonth);
            }

            return this;
        }

        public Link<TEntity> WhereNextWeek(string attributeName, string entityname = null)
        {
            if (entityname != null)
            {
                InnerLinkEntity.LinkCriteria.AddCondition(entityname, attributeName, ConditionOperator.NextWeek);
            }
            else
            {
                InnerLinkEntity.LinkCriteria.AddCondition(attributeName, ConditionOperator.NextWeek);
            }

            return this;
        }

        public Link<TEntity> WhereNextXDays(string attributeName, int value, string entityname = null)
        {
            if (entityname != null)
            {
                InnerLinkEntity.LinkCriteria.AddCondition(entityname, attributeName, ConditionOperator.NextXDays, value);
            }
            else
            {
                InnerLinkEntity.LinkCriteria.AddCondition(attributeName, ConditionOperator.NextXDays, value);
            }

            return this;
        }

        public Link<TEntity> WhereNextXFiscalPeriods(string attributeName, int value, string entityname = null)
        {
            if (entityname != null)
            {
                InnerLinkEntity.LinkCriteria.AddCondition(entityname, attributeName, ConditionOperator.NextXFiscalPeriods, value);
            }
            else
            {
                InnerLinkEntity.LinkCriteria.AddCondition(attributeName, ConditionOperator.NextXFiscalPeriods, value);
            }

            return this;
        }

        public Link<TEntity> WhereNextXFiscalYears(string attributeName, int value, string entityname = null)
        {
            if (entityname != null)
            {
                InnerLinkEntity.LinkCriteria.AddCondition(entityname, attributeName, ConditionOperator.NextXFiscalYears, value);
            }
            else
            {
                InnerLinkEntity.LinkCriteria.AddCondition(attributeName, ConditionOperator.NextXFiscalYears, value);
            }

            return this;
        }

        public Link<TEntity> WhereNextXHours(string attributeName, int value, string entityname = null)
        {
            if (entityname != null)
            {
                InnerLinkEntity.LinkCriteria.AddCondition(entityname, attributeName, ConditionOperator.NextXHours, value);
            }
            else
            {
                InnerLinkEntity.LinkCriteria.AddCondition(attributeName, ConditionOperator.NextXHours, value);
            }

            return this;
        }

        public Link<TEntity> WhereNextXMonths(string attributeName, int value, string entityname = null)
        {
            if (entityname != null)
            {
                InnerLinkEntity.LinkCriteria.AddCondition(entityname, attributeName, ConditionOperator.NextXMonths, value);
            }
            else
            {
                InnerLinkEntity.LinkCriteria.AddCondition(attributeName, ConditionOperator.NextXMonths, value);
            }

            return this;
        }

        public Link<TEntity> WhereNextXWeeks(string attributeName, int value, string entityname = null)
        {
            if (entityname != null)
            {
                InnerLinkEntity.LinkCriteria.AddCondition(entityname, attributeName, ConditionOperator.NextXWeeks, value);
            }
            else
            {
                InnerLinkEntity.LinkCriteria.AddCondition(attributeName, ConditionOperator.NextXWeeks, value);
            }

            return this;
        }

        public Link<TEntity> WhereNextXYears(string attributeName, int value, string entityname = null)
        {
            if (entityname != null)
            {
                InnerLinkEntity.LinkCriteria.AddCondition(entityname, attributeName, ConditionOperator.NextXYears, value);
            }
            else
            {
                InnerLinkEntity.LinkCriteria.AddCondition(attributeName, ConditionOperator.NextXYears, value);
            }

            return this;
        }

        public Link<TEntity> WhereNextYear(string attributeName, string entityname = null)
        {
            if (entityname != null)
            {
                InnerLinkEntity.LinkCriteria.AddCondition(entityname, attributeName, ConditionOperator.NextYear);
            }
            else
            {
                InnerLinkEntity.LinkCriteria.AddCondition(attributeName, ConditionOperator.NextYear);
            }

            return this;
        }

        public Link<TEntity> WhereNotBetween(string attributeName, object value1, object value2, string entityname = null)
        {
            if (entityname != null)
            {
                InnerLinkEntity.LinkCriteria.AddCondition(entityname, attributeName, ConditionOperator.NotBetween, value1, value2);
            }
            else
            {
                InnerLinkEntity.LinkCriteria.AddCondition(attributeName, ConditionOperator.NotBetween, value1, value2);
            }

            return this;
        }

        public Link<TEntity> WhereNotEqual(string attributeName, object value, string entityname = null)
        {
            if (entityname != null)
            {
                InnerLinkEntity.LinkCriteria.AddCondition(entityname, attributeName, ConditionOperator.NotEqual, value);
            }
            else
            {
                InnerLinkEntity.LinkCriteria.AddCondition(attributeName, ConditionOperator.NotEqual, value);
            }

            return this;
        }

        public Link<TEntity> WhereNotEqualBusinessId(string attributeName, string entityname = null)
        {
            if (entityname != null)
            {
                InnerLinkEntity.LinkCriteria.AddCondition(entityname, attributeName, ConditionOperator.NotEqualBusinessId);
            }
            else
            {
                InnerLinkEntity.LinkCriteria.AddCondition(attributeName, ConditionOperator.NotEqualBusinessId);
            }

            return this;
        }

        public Link<TEntity> WhereNotEqualUserId(string attributeName, string entityname = null)
        {
            if (entityname != null)
            {
                InnerLinkEntity.LinkCriteria.AddCondition(entityname, attributeName, ConditionOperator.NotEqualUserId);
            }
            else
            {
                InnerLinkEntity.LinkCriteria.AddCondition(attributeName, ConditionOperator.NotEqualUserId);
            }

            return this;
        }

        public Link<TEntity> WhereNotIn(string attributeName, params object[] values)
        {
            InnerLinkEntity.LinkCriteria.AddCondition(attributeName, ConditionOperator.NotIn, values);

            return this;
        }

        public Link<TEntity> WhereNotIn(string entityname, string attributeName, params object[] values)
        {
            InnerLinkEntity.LinkCriteria.AddCondition(entityname, attributeName, ConditionOperator.NotIn, values);

            return this;
        }

        public Link<TEntity> WhereNotIn(string attributeName, IList value)
        {
            InnerLinkEntity.LinkCriteria.Conditions.Add(new ConditionExpression(attributeName, ConditionOperator.NotIn, value));

            return this;
        }

        public Link<TEntity> WhereNotIn(string entityname, string attributeName, IList value)
        {
            InnerLinkEntity.LinkCriteria.Conditions.Add(new ConditionExpression(entityname, attributeName, ConditionOperator.NotIn, value));

            return this;
        }

        public Link<TEntity> WhereNotLike(string attributeName, object value, string entityname = null)
        {
            if (entityname != null)
            {
                InnerLinkEntity.LinkCriteria.AddCondition(entityname, attributeName, ConditionOperator.NotLike, value);
            }
            else
            {
                InnerLinkEntity.LinkCriteria.AddCondition(attributeName, ConditionOperator.NotLike, value);
            }

            return this;
        }

        public Link<TEntity> WhereNotMask(string attributeName, object value, string entityname = null)
        {
            if (entityname != null)
            {
                InnerLinkEntity.LinkCriteria.AddCondition(entityname, attributeName, ConditionOperator.NotMask, value);
            }
            else
            {
                InnerLinkEntity.LinkCriteria.AddCondition(attributeName, ConditionOperator.NotMask, value);
            }

            return this;
        }

        public Link<TEntity> WhereNotNull(string attributeName, string entityname = null)
        {
            if (entityname != null)
            {
                InnerLinkEntity.LinkCriteria.AddCondition(entityname, attributeName, ConditionOperator.NotNull);
            }
            else
            {
                InnerLinkEntity.LinkCriteria.AddCondition(attributeName, ConditionOperator.NotNull);
            }

            return this;
        }

        public Link<TEntity> WhereNotOn(string attributeName, object value, string entityname = null)
        {
            if (entityname != null)
            {
                InnerLinkEntity.LinkCriteria.AddCondition(entityname, attributeName, ConditionOperator.NotOn, value);
            }
            else
            {
                InnerLinkEntity.LinkCriteria.AddCondition(attributeName, ConditionOperator.NotOn, value);
            }

            return this;
        }

        public Link<TEntity> WhereNotUnder(string attributeName, object value, string entityname = null)
        {
            if (entityname != null)
            {
                InnerLinkEntity.LinkCriteria.AddCondition(entityname, attributeName, ConditionOperator.NotUnder, value);
            }
            else
            {
                InnerLinkEntity.LinkCriteria.AddCondition(attributeName, ConditionOperator.NotUnder, value);
            }

            return this;
        }

        public Link<TEntity> WhereNull(string attributeName, string entityname = null)
        {
            if (entityname != null)
            {
                InnerLinkEntity.LinkCriteria.AddCondition(entityname, attributeName, ConditionOperator.Null);
            }
            else
            {
                InnerLinkEntity.LinkCriteria.AddCondition(attributeName, ConditionOperator.Null);
            }

            return this;
        }

        public Link<TEntity> WhereOlderThanXDays(string attributeName, int value, string entityname = null)
        {
            if (entityname != null)
            {
                InnerLinkEntity.LinkCriteria.AddCondition(entityname, attributeName, ConditionOperator.OlderThanXDays, value);
            }
            else
            {
                InnerLinkEntity.LinkCriteria.AddCondition(attributeName, ConditionOperator.OlderThanXDays, value);
            }

            return this;
        }

        public Link<TEntity> WhereOlderThanXHours(string attributeName, int value, string entityname = null)
        {
            if (entityname != null)
            {
                InnerLinkEntity.LinkCriteria.AddCondition(entityname, attributeName, ConditionOperator.OlderThanXHours, value);
            }
            else
            {
                InnerLinkEntity.LinkCriteria.AddCondition(attributeName, ConditionOperator.OlderThanXHours, value);
            }

            return this;
        }

        public Link<TEntity> WhereOlderThanXMinutes(string attributeName, int value, string entityname = null)
        {
            if (entityname != null)
            {
                InnerLinkEntity.LinkCriteria.AddCondition(entityname, attributeName, ConditionOperator.OlderThanXMinutes, value);
            }
            else
            {
                InnerLinkEntity.LinkCriteria.AddCondition(attributeName, ConditionOperator.OlderThanXMinutes, value);
            }

            return this;
        }

        public Link<TEntity> WhereOlderThanXMonths(string attributeName, int value, string entityname = null)
        {
            if (entityname != null)
            {
                InnerLinkEntity.LinkCriteria.AddCondition(entityname, attributeName, ConditionOperator.OlderThanXMonths, value);
            }
            else
            {
                InnerLinkEntity.LinkCriteria.AddCondition(attributeName, ConditionOperator.OlderThanXMonths, value);
            }

            return this;
        }

        public Link<TEntity> WhereOlderThanXWeeks(string attributeName, int value, string entityname = null)
        {
            if (entityname != null)
            {
                InnerLinkEntity.LinkCriteria.AddCondition(entityname, attributeName, ConditionOperator.OlderThanXWeeks, value);
            }
            else
            {
                InnerLinkEntity.LinkCriteria.AddCondition(attributeName, ConditionOperator.OlderThanXWeeks, value);
            }

            return this;
        }

        public Link<TEntity> WhereOlderThanXYears(string attributeName, int value, string entityname = null)
        {
            if (entityname != null)
            {
                InnerLinkEntity.LinkCriteria.AddCondition(entityname, attributeName, ConditionOperator.OlderThanXYears, value);
            }
            else
            {
                InnerLinkEntity.LinkCriteria.AddCondition(attributeName, ConditionOperator.OlderThanXYears, value);
            }

            return this;
        }

        public Link<TEntity> WhereOn(string attributeName, DateTime value, string entityname = null)
        {
            if (entityname != null)
            {
                InnerLinkEntity.LinkCriteria.AddCondition(entityname, attributeName, ConditionOperator.On, value);
            }
            else
            {
                InnerLinkEntity.LinkCriteria.AddCondition(attributeName, ConditionOperator.On, value);
            }

            return this;
        }

        public Link<TEntity> WhereOnOrAfter(string attributeName, DateTime value, string entityname = null)
        {
            if (entityname != null)
            {
                InnerLinkEntity.LinkCriteria.AddCondition(entityname, attributeName, ConditionOperator.OnOrAfter, value);
            }
            else
            {
                InnerLinkEntity.LinkCriteria.AddCondition(attributeName, ConditionOperator.OnOrAfter, value);
            }

            return this;
        }

        public Link<TEntity> WhereOnOrBefore(string attributeName, DateTime value, string entityname = null)
        {
            if (entityname != null)
            {
                InnerLinkEntity.LinkCriteria.AddCondition(entityname, attributeName, ConditionOperator.OnOrBefore, value);
            }
            else
            {
                InnerLinkEntity.LinkCriteria.AddCondition(attributeName, ConditionOperator.OnOrBefore, value);
            }

            return this;
        }

        public Link<TEntity> WhereThisFiscalPeriod(string attributeName, string entityname = null)
        {
            if (entityname != null)
            {
                InnerLinkEntity.LinkCriteria.AddCondition(entityname, attributeName, ConditionOperator.ThisFiscalPeriod);
            }
            else
            {
                InnerLinkEntity.LinkCriteria.AddCondition(attributeName, ConditionOperator.ThisFiscalPeriod);
            }

            return this;
        }

        public Link<TEntity> WhereThisFiscalYear(string attributeName, string entityname = null)
        {
            if (entityname != null)
            {
                InnerLinkEntity.LinkCriteria.AddCondition(entityname, attributeName, ConditionOperator.ThisFiscalYear);
            }
            else
            {
                InnerLinkEntity.LinkCriteria.AddCondition(attributeName, ConditionOperator.ThisFiscalYear);
            }

            return this;
        }

        public Link<TEntity> WhereThisMonth(string attributeName, string entityname = null)
        {
            if (entityname != null)
            {
                InnerLinkEntity.LinkCriteria.AddCondition(entityname, attributeName, ConditionOperator.ThisMonth);
            }
            else
            {
                InnerLinkEntity.LinkCriteria.AddCondition(attributeName, ConditionOperator.ThisMonth);
            }

            return this;
        }

        public Link<TEntity> WhereThisWeek(string attributeName, string entityname = null)
        {
            if (entityname != null)
            {
                InnerLinkEntity.LinkCriteria.AddCondition(entityname, attributeName, ConditionOperator.ThisWeek);
            }
            else
            {
                InnerLinkEntity.LinkCriteria.AddCondition(attributeName, ConditionOperator.ThisWeek);
            }

            return this;
        }

        public Link<TEntity> WhereThisYear(string attributeName, string entityname = null)
        {
            if (entityname != null)
            {
                InnerLinkEntity.LinkCriteria.AddCondition(entityname, attributeName, ConditionOperator.ThisYear);
            }
            else
            {
                InnerLinkEntity.LinkCriteria.AddCondition(attributeName, ConditionOperator.ThisYear);
            }

            return this;
        }

        public Link<TEntity> WhereToday(string attributeName, string entityname = null)
        {
            if (entityname != null)
            {
                InnerLinkEntity.LinkCriteria.AddCondition(entityname, attributeName, ConditionOperator.Today);
            }
            else
            {
                InnerLinkEntity.LinkCriteria.AddCondition(attributeName, ConditionOperator.Today);
            }

            return this;
        }

        public Link<TEntity> WhereTomorrow(string attributeName, string entityname = null)
        {
            if (entityname != null)
            {
                InnerLinkEntity.LinkCriteria.AddCondition(entityname, attributeName, ConditionOperator.Tomorrow);
            }
            else
            {
                InnerLinkEntity.LinkCriteria.AddCondition(attributeName, ConditionOperator.Tomorrow);
            }

            return this;
        }

        public Link<TEntity> WhereUnder(string attributeName, object value, string entityname = null)
        {
            if (entityname != null)
            {
                InnerLinkEntity.LinkCriteria.AddCondition(entityname, attributeName, ConditionOperator.Under, value);
            }
            else
            {
                InnerLinkEntity.LinkCriteria.AddCondition(attributeName, ConditionOperator.Under, value);
            }

            return this;
        }

        public Link<TEntity> WhereUnderOrEqual(string attributeName, object value, string entityname = null)
        {
            if (entityname != null)
            {
                InnerLinkEntity.LinkCriteria.AddCondition(entityname, attributeName, ConditionOperator.UnderOrEqual, value);
            }
            else
            {
                InnerLinkEntity.LinkCriteria.AddCondition(attributeName, ConditionOperator.UnderOrEqual, value);
            }

            return this;
        }

        public Link<TEntity> WhereYesterday(string attributeName, string entityname = null)
        {
            if (entityname != null)
            {
                InnerLinkEntity.LinkCriteria.AddCondition(entityname, attributeName, ConditionOperator.Yesterday);
            }
            else
            {
                InnerLinkEntity.LinkCriteria.AddCondition(attributeName, ConditionOperator.Yesterday);
            }

            return this;
        }

        #endregion Conditions
    }
}