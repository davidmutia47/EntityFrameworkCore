﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.TestUtilities;
using Xunit.Abstractions;

namespace Microsoft.EntityFrameworkCore.Query
{
    public class SimpleQuerySqliteTest : SimpleQueryTestBase<NorthwindQuerySqliteFixture<NoopModelCustomizer>>
    {
        // ReSharper disable once UnusedParameter.Local
        public SimpleQuerySqliteTest(NorthwindQuerySqliteFixture<NoopModelCustomizer> fixture, ITestOutputHelper testOutputHelper)
            : base(fixture)
        {
            Fixture.TestSqlLoggerFactory.Clear();
            //Fixture.TestSqlLoggerFactory.SetTestOutputHelper(testOutputHelper);
        }

        public override void Query_backed_by_database_view()
        {
            // Not present on SQLite
        }

        public override async Task Take_Skip()
        {
            await base.Take_Skip();

            AssertSql(
                @"@__p_0='10' (DbType = String)
@__p_1='5' (DbType = String)

SELECT ""t"".*
FROM (
    SELECT ""c"".""CustomerID"", ""c"".""Address"", ""c"".""City"", ""c"".""CompanyName"", ""c"".""ContactName"", ""c"".""ContactTitle"", ""c"".""Country"", ""c"".""Fax"", ""c"".""Phone"", ""c"".""PostalCode"", ""c"".""Region""
    FROM ""Customers"" AS ""c""
    ORDER BY ""c"".""ContactName""
    LIMIT @__p_0
) AS ""t""
ORDER BY ""t"".""ContactName""
LIMIT -1 OFFSET @__p_1");
        }

        public override async Task Where_datetime_now()
        {
            await base.Where_datetime_now();

            AssertSql(
                @"@__myDatetime_0='2015-04-10T00:00:00' (DbType = String)

SELECT ""c"".""CustomerID"", ""c"".""Address"", ""c"".""City"", ""c"".""CompanyName"", ""c"".""ContactName"", ""c"".""ContactTitle"", ""c"".""Country"", ""c"".""Fax"", ""c"".""Phone"", ""c"".""PostalCode"", ""c"".""Region""
FROM ""Customers"" AS ""c""
WHERE rtrim(rtrim(strftime('%Y-%m-%d %H:%M:%f', 'now', 'localtime'), '0'), '.') <> @__myDatetime_0");
        }

        public override async Task Where_datetime_utcnow()
        {
            await base.Where_datetime_utcnow();

            AssertSql(
                @"@__myDatetime_0='2015-04-10T00:00:00' (DbType = String)

SELECT ""c"".""CustomerID"", ""c"".""Address"", ""c"".""City"", ""c"".""CompanyName"", ""c"".""ContactName"", ""c"".""ContactTitle"", ""c"".""Country"", ""c"".""Fax"", ""c"".""Phone"", ""c"".""PostalCode"", ""c"".""Region""
FROM ""Customers"" AS ""c""
WHERE rtrim(rtrim(strftime('%Y-%m-%d %H:%M:%f', 'now'), '0'), '.') <> @__myDatetime_0");
        }

        public override async Task Where_datetime_today()
        {
            await base.Where_datetime_today();

            AssertSql(
                @"SELECT ""e"".""EmployeeID"", ""e"".""City"", ""e"".""Country"", ""e"".""FirstName"", ""e"".""ReportsTo"", ""e"".""Title""
FROM ""Employees"" AS ""e""
WHERE rtrim(rtrim(strftime('%Y-%m-%d %H:%M:%f', rtrim(rtrim(strftime('%Y-%m-%d %H:%M:%f', 'now', 'localtime'), '0'), '.'), 'start of day'), '0'), '.') = rtrim(rtrim(strftime('%Y-%m-%d %H:%M:%f', 'now', 'localtime', 'start of day'), '0'), '.')");
        }

        public override async Task Where_datetime_date_component()
        {
            await base.Where_datetime_date_component();

            AssertSql(
                @"@__myDatetime_0='1998-05-04T00:00:00' (DbType = String)

SELECT ""o"".""OrderID"", ""o"".""CustomerID"", ""o"".""EmployeeID"", ""o"".""OrderDate""
FROM ""Orders"" AS ""o""
WHERE rtrim(rtrim(strftime('%Y-%m-%d %H:%M:%f', ""o"".""OrderDate"", 'start of day'), '0'), '.') = @__myDatetime_0");
        }

        public override async Task Where_datetime_year_component()
        {
            await base.Where_datetime_year_component();

            AssertSql(
                @"SELECT ""o"".""OrderID"", ""o"".""CustomerID"", ""o"".""EmployeeID"", ""o"".""OrderDate""
FROM ""Orders"" AS ""o""
WHERE CAST(strftime('%Y', ""o"".""OrderDate"") AS INTEGER) = 1998");
        }

        public override async Task Where_datetime_month_component()
        {
            await base.Where_datetime_month_component();

            AssertSql(
                @"SELECT ""o"".""OrderID"", ""o"".""CustomerID"", ""o"".""EmployeeID"", ""o"".""OrderDate""
FROM ""Orders"" AS ""o""
WHERE CAST(strftime('%m', ""o"".""OrderDate"") AS INTEGER) = 4");
        }

        public override async Task Where_datetime_dayOfYear_component()
        {
            await base.Where_datetime_dayOfYear_component();

            AssertSql(
                @"SELECT ""o"".""OrderID"", ""o"".""CustomerID"", ""o"".""EmployeeID"", ""o"".""OrderDate""
FROM ""Orders"" AS ""o""
WHERE CAST(strftime('%j', ""o"".""OrderDate"") AS INTEGER) = 68");
        }

        public override async Task Where_datetime_day_component()
        {
            await base.Where_datetime_day_component();

            AssertSql(
                @"SELECT ""o"".""OrderID"", ""o"".""CustomerID"", ""o"".""EmployeeID"", ""o"".""OrderDate""
FROM ""Orders"" AS ""o""
WHERE CAST(strftime('%d', ""o"".""OrderDate"") AS INTEGER) = 4");
        }

        public override async Task Where_datetime_hour_component()
        {
            await base.Where_datetime_hour_component();

            AssertSql(
                @"SELECT ""o"".""OrderID"", ""o"".""CustomerID"", ""o"".""EmployeeID"", ""o"".""OrderDate""
FROM ""Orders"" AS ""o""
WHERE CAST(strftime('%H', ""o"".""OrderDate"") AS INTEGER) = 14");
        }

        public override async Task Where_datetime_minute_component()
        {
            await base.Where_datetime_minute_component();

            AssertSql(
                @"SELECT ""o"".""OrderID"", ""o"".""CustomerID"", ""o"".""EmployeeID"", ""o"".""OrderDate""
FROM ""Orders"" AS ""o""
WHERE CAST(strftime('%M', ""o"".""OrderDate"") AS INTEGER) = 23");
        }

        public override async Task Where_datetime_second_component()
        {
            await base.Where_datetime_second_component();

            AssertSql(
                @"SELECT ""o"".""OrderID"", ""o"".""CustomerID"", ""o"".""EmployeeID"", ""o"".""OrderDate""
FROM ""Orders"" AS ""o""
WHERE CAST(strftime('%S', ""o"".""OrderDate"") AS INTEGER) = 44");
        }

        public override async Task Where_datetime_millisecond_component()
        {
            await base.Where_datetime_millisecond_component();

            AssertSql(
                @"SELECT ""o"".""OrderID"", ""o"".""CustomerID"", ""o"".""EmployeeID"", ""o"".""OrderDate""
FROM ""Orders"" AS ""o""
WHERE ((CAST(strftime('%f', ""o"".""OrderDate"") AS REAL) * 1000) % 1000) = 88");
        }

        public override async Task String_StartsWith_Literal()
        {
            await base.String_StartsWith_Literal();

            AssertSql(
                @"SELECT ""c"".""CustomerID"", ""c"".""Address"", ""c"".""City"", ""c"".""CompanyName"", ""c"".""ContactName"", ""c"".""ContactTitle"", ""c"".""Country"", ""c"".""Fax"", ""c"".""Phone"", ""c"".""PostalCode"", ""c"".""Region""
FROM ""Customers"" AS ""c""
WHERE ""c"".""ContactName"" LIKE 'M' || '%' AND (substr(""c"".""ContactName"", 1, length('M')) = 'M')");
        }

        public override async Task String_StartsWith_Identity()
        {
            await base.String_StartsWith_Identity();

            AssertSql(
                @"SELECT ""c"".""CustomerID"", ""c"".""Address"", ""c"".""City"", ""c"".""CompanyName"", ""c"".""ContactName"", ""c"".""ContactTitle"", ""c"".""Country"", ""c"".""Fax"", ""c"".""Phone"", ""c"".""PostalCode"", ""c"".""Region""
FROM ""Customers"" AS ""c""
WHERE (""c"".""ContactName"" LIKE ""c"".""ContactName"" || '%' AND (substr(""c"".""ContactName"", 1, length(""c"".""ContactName"")) = ""c"".""ContactName"")) OR (""c"".""ContactName"" = '')");
        }

        public override async Task String_StartsWith_Column()
        {
            await base.String_StartsWith_Column();

            AssertSql(
                @"SELECT ""c"".""CustomerID"", ""c"".""Address"", ""c"".""City"", ""c"".""CompanyName"", ""c"".""ContactName"", ""c"".""ContactTitle"", ""c"".""Country"", ""c"".""Fax"", ""c"".""Phone"", ""c"".""PostalCode"", ""c"".""Region""
FROM ""Customers"" AS ""c""
WHERE (""c"".""ContactName"" LIKE ""c"".""ContactName"" || '%' AND (substr(""c"".""ContactName"", 1, length(""c"".""ContactName"")) = ""c"".""ContactName"")) OR (""c"".""ContactName"" = '')");
        }

        public override async Task String_StartsWith_MethodCall()
        {
            await base.String_StartsWith_MethodCall();

            AssertSql(
                @"@__LocalMethod1_0='M' (Size = 1)

SELECT ""c"".""CustomerID"", ""c"".""Address"", ""c"".""City"", ""c"".""CompanyName"", ""c"".""ContactName"", ""c"".""ContactTitle"", ""c"".""Country"", ""c"".""Fax"", ""c"".""Phone"", ""c"".""PostalCode"", ""c"".""Region""
FROM ""Customers"" AS ""c""
WHERE (""c"".""ContactName"" LIKE @__LocalMethod1_0 || '%' AND (substr(""c"".""ContactName"", 1, length(@__LocalMethod1_0)) = @__LocalMethod1_0)) OR (@__LocalMethod1_0 = '')");
        }

        public override async Task String_EndsWith_Literal()
        {
            await base.String_EndsWith_Literal();

            AssertSql(
                @"SELECT ""c"".""CustomerID"", ""c"".""Address"", ""c"".""City"", ""c"".""CompanyName"", ""c"".""ContactName"", ""c"".""ContactTitle"", ""c"".""Country"", ""c"".""Fax"", ""c"".""Phone"", ""c"".""PostalCode"", ""c"".""Region""
FROM ""Customers"" AS ""c""
WHERE substr(""c"".""ContactName"", -length('b')) = 'b'");
        }

        public override async Task String_EndsWith_Identity()
        {
            await base.String_EndsWith_Identity();

            AssertSql(
                @"SELECT ""c"".""CustomerID"", ""c"".""Address"", ""c"".""City"", ""c"".""CompanyName"", ""c"".""ContactName"", ""c"".""ContactTitle"", ""c"".""Country"", ""c"".""Fax"", ""c"".""Phone"", ""c"".""PostalCode"", ""c"".""Region""
FROM ""Customers"" AS ""c""
WHERE (substr(""c"".""ContactName"", -length(""c"".""ContactName"")) = ""c"".""ContactName"") OR (""c"".""ContactName"" = '')");
        }

        public override async Task String_EndsWith_Column()
        {
            await base.String_EndsWith_Column();

            AssertSql(
                @"SELECT ""c"".""CustomerID"", ""c"".""Address"", ""c"".""City"", ""c"".""CompanyName"", ""c"".""ContactName"", ""c"".""ContactTitle"", ""c"".""Country"", ""c"".""Fax"", ""c"".""Phone"", ""c"".""PostalCode"", ""c"".""Region""
FROM ""Customers"" AS ""c""
WHERE (substr(""c"".""ContactName"", -length(""c"".""ContactName"")) = ""c"".""ContactName"") OR (""c"".""ContactName"" = '')");
        }

        public override async Task String_EndsWith_MethodCall()
        {
            await base.String_EndsWith_MethodCall();

            AssertSql(
                @"@__LocalMethod2_0='m' (Size = 1)

SELECT ""c"".""CustomerID"", ""c"".""Address"", ""c"".""City"", ""c"".""CompanyName"", ""c"".""ContactName"", ""c"".""ContactTitle"", ""c"".""Country"", ""c"".""Fax"", ""c"".""Phone"", ""c"".""PostalCode"", ""c"".""Region""
FROM ""Customers"" AS ""c""
WHERE (substr(""c"".""ContactName"", -length(@__LocalMethod2_0)) = @__LocalMethod2_0) OR (@__LocalMethod2_0 = '')");
        }

        public override async Task String_Contains_Literal()
        {
            await base.String_Contains_Literal();

            AssertSql(
                @"SELECT ""c"".""CustomerID"", ""c"".""Address"", ""c"".""City"", ""c"".""CompanyName"", ""c"".""ContactName"", ""c"".""ContactTitle"", ""c"".""Country"", ""c"".""Fax"", ""c"".""Phone"", ""c"".""PostalCode"", ""c"".""Region""
FROM ""Customers"" AS ""c""
WHERE instr(""c"".""ContactName"", 'M') > 0");
        }

        public override async Task String_Contains_Identity()
        {
            await base.String_Contains_Identity();

            AssertSql(
                @"SELECT ""c"".""CustomerID"", ""c"".""Address"", ""c"".""City"", ""c"".""CompanyName"", ""c"".""ContactName"", ""c"".""ContactTitle"", ""c"".""Country"", ""c"".""Fax"", ""c"".""Phone"", ""c"".""PostalCode"", ""c"".""Region""
FROM ""Customers"" AS ""c""
WHERE (instr(""c"".""ContactName"", ""c"".""ContactName"") > 0) OR (""c"".""ContactName"" = '')");
        }

        public override async Task String_Contains_Column()
        {
            await base.String_Contains_Column();

            AssertSql(
                @"SELECT ""c"".""CustomerID"", ""c"".""Address"", ""c"".""City"", ""c"".""CompanyName"", ""c"".""ContactName"", ""c"".""ContactTitle"", ""c"".""Country"", ""c"".""Fax"", ""c"".""Phone"", ""c"".""PostalCode"", ""c"".""Region""
FROM ""Customers"" AS ""c""
WHERE (instr(""c"".""ContactName"", ""c"".""ContactName"") > 0) OR (""c"".""ContactName"" = '')");
        }

        public override async Task String_Contains_MethodCall()
        {
            await base.String_Contains_MethodCall();

            AssertSql(
                @"@__LocalMethod1_0='M' (Size = 1)

SELECT ""c"".""CustomerID"", ""c"".""Address"", ""c"".""City"", ""c"".""CompanyName"", ""c"".""ContactName"", ""c"".""ContactTitle"", ""c"".""Country"", ""c"".""Fax"", ""c"".""Phone"", ""c"".""PostalCode"", ""c"".""Region""
FROM ""Customers"" AS ""c""
WHERE (instr(""c"".""ContactName"", @__LocalMethod1_0) > 0) OR (@__LocalMethod1_0 = '')");
        }

        public override async Task IsNullOrWhiteSpace_in_predicate()
        {
            await base.IsNullOrWhiteSpace_in_predicate();

            AssertSql(
                @"SELECT ""c"".""CustomerID"", ""c"".""Address"", ""c"".""City"", ""c"".""CompanyName"", ""c"".""ContactName"", ""c"".""ContactTitle"", ""c"".""Country"", ""c"".""Fax"", ""c"".""Phone"", ""c"".""PostalCode"", ""c"".""Region""
FROM ""Customers"" AS ""c""
WHERE ""c"".""Region"" IS NULL OR (trim(""c"".""Region"") = '')");
        }

        public override async Task Where_string_length()
        {
            await base.Where_string_length();

            AssertSql(
                @"SELECT ""c"".""CustomerID"", ""c"".""Address"", ""c"".""City"", ""c"".""CompanyName"", ""c"".""ContactName"", ""c"".""ContactTitle"", ""c"".""Country"", ""c"".""Fax"", ""c"".""Phone"", ""c"".""PostalCode"", ""c"".""Region""
FROM ""Customers"" AS ""c""
WHERE length(""c"".""City"") = 6");
        }

        public override async Task Where_string_indexof()
        {
            await base.Where_string_indexof();

            AssertSql(
                @"SELECT ""c"".""CustomerID"", ""c"".""Address"", ""c"".""City"", ""c"".""CompanyName"", ""c"".""ContactName"", ""c"".""ContactTitle"", ""c"".""Country"", ""c"".""Fax"", ""c"".""Phone"", ""c"".""PostalCode"", ""c"".""Region""
FROM ""Customers"" AS ""c""
WHERE (instr(""c"".""City"", 'Sea') - 1) <> -1");
        }

        public override void Indexof_with_emptystring()
        {
            base.Indexof_with_emptystring();

            AssertSql(
                @"SELECT instr(""c"".""ContactName"", '') - 1
FROM ""Customers"" AS ""c""
WHERE ""c"".""CustomerID"" = 'ALFKI'");
        }

        public override async Task Where_string_replace()
        {
            await base.Where_string_replace();

            AssertSql(
                @"SELECT ""c"".""CustomerID"", ""c"".""Address"", ""c"".""City"", ""c"".""CompanyName"", ""c"".""ContactName"", ""c"".""ContactTitle"", ""c"".""Country"", ""c"".""Fax"", ""c"".""Phone"", ""c"".""PostalCode"", ""c"".""Region""
FROM ""Customers"" AS ""c""
WHERE replace(""c"".""City"", 'Sea', 'Rea') = 'Reattle'");
        }

        public override void Replace_with_emptystring()
        {
            base.Replace_with_emptystring();

            AssertSql(
                @"SELECT replace(""c"".""ContactName"", 'ari', '')
FROM ""Customers"" AS ""c""
WHERE ""c"".""CustomerID"" = 'ALFKI'");
        }

        public override async Task Where_string_substring()
        {
            await base.Where_string_substring();

            AssertSql(
                @"SELECT ""c"".""CustomerID"", ""c"".""Address"", ""c"".""City"", ""c"".""CompanyName"", ""c"".""ContactName"", ""c"".""ContactTitle"", ""c"".""Country"", ""c"".""Fax"", ""c"".""Phone"", ""c"".""PostalCode"", ""c"".""Region""
FROM ""Customers"" AS ""c""
WHERE substr(""c"".""City"", 2, 2) = 'ea'");
        }

        public override void Substring_with_zero_startindex()
        {
            base.Substring_with_zero_startindex();

            AssertSql(
                @"SELECT substr(""c"".""ContactName"", 1, 3)
FROM ""Customers"" AS ""c""
WHERE ""c"".""CustomerID"" = 'ALFKI'");
        }

        public override void Substring_with_constant()
        {
            base.Substring_with_constant();

            AssertSql(
                @"SELECT substr(""c"".""ContactName"", 2, 3)
FROM ""Customers"" AS ""c""
WHERE ""c"".""CustomerID"" = 'ALFKI'");
        }

        public override void Substring_with_closure()
        {
            base.Substring_with_closure();

            AssertSql(
                @"@__start_0='2' (DbType = String)

SELECT substr(""c"".""ContactName"", @__start_0 + 1, 3)
FROM ""Customers"" AS ""c""
WHERE ""c"".""CustomerID"" = 'ALFKI'");
        }

        public override void Substring_with_client_eval()
        {
            base.Substring_with_client_eval();

            AssertSql(
                @"SELECT ""c"".""ContactName""
FROM ""Customers"" AS ""c""
WHERE ""c"".""CustomerID"" = 'ALFKI'");
        }

        public override void Substring_with_zero_length()
        {
            base.Substring_with_zero_length();

            AssertSql(
                @"SELECT substr(""c"".""ContactName"", 3, 0)
FROM ""Customers"" AS ""c""
WHERE ""c"".""CustomerID"" = 'ALFKI'");
        }

        public override async Task Where_math_abs1()
        {
            await base.Where_math_abs1();

            AssertSql(
                @"SELECT ""od"".""OrderID"", ""od"".""ProductID"", ""od"".""Discount"", ""od"".""Quantity"", ""od"".""UnitPrice""
FROM ""Order Details"" AS ""od""
WHERE abs(""od"".""ProductID"") > 10");
        }

        public override async Task Where_math_abs2()
        {
            await base.Where_math_abs2();

            AssertSql(
                @"SELECT ""od"".""OrderID"", ""od"".""ProductID"", ""od"".""Discount"", ""od"".""Quantity"", ""od"".""UnitPrice""
FROM ""Order Details"" AS ""od""
WHERE abs(""od"".""Quantity"") > 10");
        }

        public override async Task Where_math_abs_uncorrelated()
        {
            await base.Where_math_abs_uncorrelated();

            AssertSql(
                @"@__Abs_0='10' (DbType = String)

SELECT ""od"".""OrderID"", ""od"".""ProductID"", ""od"".""Discount"", ""od"".""Quantity"", ""od"".""UnitPrice""
FROM ""Order Details"" AS ""od""
WHERE @__Abs_0 < ""od"".""ProductID""");
        }

        public override async Task Select_math_round_int()
        {
            await base.Select_math_round_int();

            AssertSql(
                @"SELECT round(""o"".""OrderID"") AS ""A""
FROM ""Orders"" AS ""o""
WHERE ""o"".""OrderID"" < 10250");
        }

        public override async Task Where_math_min()
        {
            await base.Where_math_min();

            AssertSql(
                @"SELECT ""od"".""OrderID"", ""od"".""ProductID"", ""od"".""Discount"", ""od"".""Quantity"", ""od"".""UnitPrice""
FROM ""Order Details"" AS ""od""
WHERE (""od"".""OrderID"" = 11077) AND (min(""od"".""OrderID"", ""od"".""ProductID"") = ""od"".""ProductID"")");
        }

        public override async Task Where_math_max()
        {
            await base.Where_math_max();

            AssertSql(
                @"SELECT ""od"".""OrderID"", ""od"".""ProductID"", ""od"".""Discount"", ""od"".""Quantity"", ""od"".""UnitPrice""
FROM ""Order Details"" AS ""od""
WHERE (""od"".""OrderID"" = 11077) AND (max(""od"".""OrderID"", ""od"".""ProductID"") = ""od"".""OrderID"")");
        }

        public override async Task Where_string_to_lower()
        {
            await base.Where_string_to_lower();

            AssertSql(
                @"SELECT ""c"".""CustomerID"", ""c"".""Address"", ""c"".""City"", ""c"".""CompanyName"", ""c"".""ContactName"", ""c"".""ContactTitle"", ""c"".""Country"", ""c"".""Fax"", ""c"".""Phone"", ""c"".""PostalCode"", ""c"".""Region""
FROM ""Customers"" AS ""c""
WHERE lower(""c"".""CustomerID"") = 'alfki'");
        }

        public override async Task Where_string_to_upper()
        {
            await base.Where_string_to_upper();

            AssertSql(
                @"SELECT ""c"".""CustomerID"", ""c"".""Address"", ""c"".""City"", ""c"".""CompanyName"", ""c"".""ContactName"", ""c"".""ContactTitle"", ""c"".""Country"", ""c"".""Fax"", ""c"".""Phone"", ""c"".""PostalCode"", ""c"".""Region""
FROM ""Customers"" AS ""c""
WHERE upper(""c"".""CustomerID"") = 'ALFKI'");
        }

        public override async Task TrimStart_without_arguments_in_predicate()
        {
            await base.TrimStart_without_arguments_in_predicate();

            AssertSql(
                @"SELECT ""c"".""CustomerID"", ""c"".""Address"", ""c"".""City"", ""c"".""CompanyName"", ""c"".""ContactName"", ""c"".""ContactTitle"", ""c"".""Country"", ""c"".""Fax"", ""c"".""Phone"", ""c"".""PostalCode"", ""c"".""Region""
FROM ""Customers"" AS ""c""
WHERE ltrim(""c"".""ContactTitle"") = 'Owner'");
        }

        public override async Task TrimStart_with_char_argument_in_predicate()
        {
            await base.TrimStart_with_char_argument_in_predicate();

            AssertSql(
                @"SELECT ""c"".""CustomerID"", ""c"".""Address"", ""c"".""City"", ""c"".""CompanyName"", ""c"".""ContactName"", ""c"".""ContactTitle"", ""c"".""Country"", ""c"".""Fax"", ""c"".""Phone"", ""c"".""PostalCode"", ""c"".""Region""
FROM ""Customers"" AS ""c""
WHERE ltrim(""c"".""ContactTitle"", 'O') = 'wner'");
        }

        public override async Task TrimStart_with_char_array_argument_in_predicate()
        {
            await base.TrimStart_with_char_array_argument_in_predicate();

            AssertSql(
                @"SELECT ""c"".""CustomerID"", ""c"".""Address"", ""c"".""City"", ""c"".""CompanyName"", ""c"".""ContactName"", ""c"".""ContactTitle"", ""c"".""Country"", ""c"".""Fax"", ""c"".""Phone"", ""c"".""PostalCode"", ""c"".""Region""
FROM ""Customers"" AS ""c""
WHERE ltrim(""c"".""ContactTitle"", 'Ow') = 'ner'");
        }

        public override async Task TrimEnd_without_arguments_in_predicate()
        {
            await base.TrimEnd_without_arguments_in_predicate();

            AssertSql(
                @"SELECT ""c"".""CustomerID"", ""c"".""Address"", ""c"".""City"", ""c"".""CompanyName"", ""c"".""ContactName"", ""c"".""ContactTitle"", ""c"".""Country"", ""c"".""Fax"", ""c"".""Phone"", ""c"".""PostalCode"", ""c"".""Region""
FROM ""Customers"" AS ""c""
WHERE rtrim(""c"".""ContactTitle"") = 'Owner'");
        }

        public override async Task TrimEnd_with_char_argument_in_predicate()
        {
            await base.TrimEnd_with_char_argument_in_predicate();

            AssertSql(
                @"SELECT ""c"".""CustomerID"", ""c"".""Address"", ""c"".""City"", ""c"".""CompanyName"", ""c"".""ContactName"", ""c"".""ContactTitle"", ""c"".""Country"", ""c"".""Fax"", ""c"".""Phone"", ""c"".""PostalCode"", ""c"".""Region""
FROM ""Customers"" AS ""c""
WHERE rtrim(""c"".""ContactTitle"", 'r') = 'Owne'");
        }

        public override async Task TrimEnd_with_char_array_argument_in_predicate()
        {
            await base.TrimEnd_with_char_array_argument_in_predicate();

            AssertSql(
                @"SELECT ""c"".""CustomerID"", ""c"".""Address"", ""c"".""City"", ""c"".""CompanyName"", ""c"".""ContactName"", ""c"".""ContactTitle"", ""c"".""Country"", ""c"".""Fax"", ""c"".""Phone"", ""c"".""PostalCode"", ""c"".""Region""
FROM ""Customers"" AS ""c""
WHERE rtrim(""c"".""ContactTitle"", 'er') = 'Own'");
        }

        public override async Task Trim_without_argument_in_predicate()
        {
            await base.Trim_without_argument_in_predicate();

            AssertSql(
                @"SELECT ""c"".""CustomerID"", ""c"".""Address"", ""c"".""City"", ""c"".""CompanyName"", ""c"".""ContactName"", ""c"".""ContactTitle"", ""c"".""Country"", ""c"".""Fax"", ""c"".""Phone"", ""c"".""PostalCode"", ""c"".""Region""
FROM ""Customers"" AS ""c""
WHERE trim(""c"".""ContactTitle"") = 'Owner'");
        }

        public override async Task Trim_with_char_argument_in_predicate()
        {
            await base.Trim_with_char_argument_in_predicate();

            AssertSql(
                @"SELECT ""c"".""CustomerID"", ""c"".""Address"", ""c"".""City"", ""c"".""CompanyName"", ""c"".""ContactName"", ""c"".""ContactTitle"", ""c"".""Country"", ""c"".""Fax"", ""c"".""Phone"", ""c"".""PostalCode"", ""c"".""Region""
FROM ""Customers"" AS ""c""
WHERE trim(""c"".""ContactTitle"", 'O') = 'wner'");
        }

        public override async Task Trim_with_char_array_argument_in_predicate()
        {
            await base.Trim_with_char_array_argument_in_predicate();

            AssertSql(
                @"SELECT ""c"".""CustomerID"", ""c"".""Address"", ""c"".""City"", ""c"".""CompanyName"", ""c"".""ContactName"", ""c"".""ContactTitle"", ""c"".""Country"", ""c"".""Fax"", ""c"".""Phone"", ""c"".""PostalCode"", ""c"".""Region""
FROM ""Customers"" AS ""c""
WHERE trim(""c"".""ContactTitle"", 'Or') = 'wne'");
        }

        public override void Sum_with_coalesce()
        {
            base.Sum_with_coalesce();

            AssertSql(
                @"SELECT COALESCE(""p"".""UnitPrice"", '0.0')
FROM ""Products"" AS ""p""
WHERE ""p"".""ProductID"" < 40");
        }

        public override async Task Select_datetime_year_component()
        {
            await base.Select_datetime_year_component();

            AssertSql(
                @"SELECT CAST(strftime('%Y', ""o"".""OrderDate"") AS INTEGER)
FROM ""Orders"" AS ""o""");
        }

        public override async Task Select_datetime_month_component()
        {
            await base.Select_datetime_month_component();

            AssertSql(
                @"SELECT CAST(strftime('%m', ""o"".""OrderDate"") AS INTEGER)
FROM ""Orders"" AS ""o""");
        }

        public override async Task Select_datetime_day_of_year_component()
        {
            await base.Select_datetime_day_of_year_component();

            AssertSql(
                @"SELECT CAST(strftime('%j', ""o"".""OrderDate"") AS INTEGER)
FROM ""Orders"" AS ""o""");
        }

        public override async Task Select_datetime_day_component()
        {
            await base.Select_datetime_day_component();

            AssertSql(
                @"SELECT CAST(strftime('%d', ""o"".""OrderDate"") AS INTEGER)
FROM ""Orders"" AS ""o""");
        }

        public override async Task Select_datetime_hour_component()
        {
            await base.Select_datetime_hour_component();

            AssertSql(
                @"SELECT CAST(strftime('%H', ""o"".""OrderDate"") AS INTEGER)
FROM ""Orders"" AS ""o""");
        }

        public override async Task Select_datetime_minute_component()
        {
            await base.Select_datetime_minute_component();

            AssertSql(
                @"SELECT CAST(strftime('%M', ""o"".""OrderDate"") AS INTEGER)
FROM ""Orders"" AS ""o""");
        }

        public override async Task Select_datetime_second_component()
        {
            await base.Select_datetime_second_component();

            AssertSql(
                @"SELECT CAST(strftime('%S', ""o"".""OrderDate"") AS INTEGER)
FROM ""Orders"" AS ""o""");
        }

        public override async Task Select_datetime_millisecond_component()
        {
            await base.Select_datetime_millisecond_component();

            AssertSql(
                @"SELECT (CAST(strftime('%f', ""o"".""OrderDate"") AS REAL) * 1000) % 1000
FROM ""Orders"" AS ""o""");
        }

        public override async Task Select_datetime_DayOfWeek_component()
        {
            await base.Select_datetime_DayOfWeek_component();

            AssertSql(
                @"SELECT CAST(strftime('%w', ""o"".""OrderDate"") AS INTEGER)
FROM ""Orders"" AS ""o""");
        }

        public override async Task Select_datetime_Ticks_component()
        {
            await base.Select_datetime_Ticks_component();

            AssertSql(
                @"SELECT CAST((julianday(""o"".""OrderDate"") - 1721425.5) * 864000000000 AS INTEGER)
FROM ""Orders"" AS ""o""");
        }

        public override async Task Select_datetime_TimeOfDay_component()
        {
            await base.Select_datetime_TimeOfDay_component();

            AssertSql(
                @"SELECT rtrim(rtrim(strftime('%H:%M:%f', ""o"".""OrderDate""), '0'), '.')
FROM ""Orders"" AS ""o""");
        }

        public override async Task Select_expression_date_add_year()
        {
            await base.Select_expression_date_add_year();

            AssertSql(
                @"SELECT rtrim(rtrim(strftime('%Y-%m-%d %H:%M:%f', ""o"".""OrderDate"", CAST(1 AS TEXT) || ' years'), '0'), '.') AS ""OrderDate""
FROM ""Orders"" AS ""o""
WHERE ""o"".""OrderDate"" IS NOT NULL");
        }

        public override async Task Select_expression_datetime_add_month()
        {
            await base.Select_expression_datetime_add_month();

            AssertSql(
                @"SELECT rtrim(rtrim(strftime('%Y-%m-%d %H:%M:%f', ""o"".""OrderDate"", CAST(1 AS TEXT) || ' months'), '0'), '.') AS ""OrderDate""
FROM ""Orders"" AS ""o""
WHERE ""o"".""OrderDate"" IS NOT NULL");
        }

        public override async Task Select_expression_datetime_add_hour()
        {
            await base.Select_expression_datetime_add_hour();

            AssertSql(
                @"SELECT rtrim(rtrim(strftime('%Y-%m-%d %H:%M:%f', ""o"".""OrderDate"", CAST(1.0 AS TEXT) || ' hours'), '0'), '.') AS ""OrderDate""
FROM ""Orders"" AS ""o""
WHERE ""o"".""OrderDate"" IS NOT NULL");
        }

        public override async Task Select_expression_datetime_add_minute()
        {
            await base.Select_expression_datetime_add_minute();

            AssertSql(
                @"SELECT rtrim(rtrim(strftime('%Y-%m-%d %H:%M:%f', ""o"".""OrderDate"", CAST(1.0 AS TEXT) || ' minutes'), '0'), '.') AS ""OrderDate""
FROM ""Orders"" AS ""o""
WHERE ""o"".""OrderDate"" IS NOT NULL");
        }

        public override async Task Select_expression_datetime_add_second()
        {
            await base.Select_expression_datetime_add_second();

            AssertSql(
                @"SELECT rtrim(rtrim(strftime('%Y-%m-%d %H:%M:%f', ""o"".""OrderDate"", CAST(1.0 AS TEXT) || ' seconds'), '0'), '.') AS ""OrderDate""
FROM ""Orders"" AS ""o""
WHERE ""o"".""OrderDate"" IS NOT NULL");
        }

        public override async Task Select_expression_datetime_add_ticks()
        {
            await base.Select_expression_datetime_add_ticks();

            AssertSql(
                @"SELECT rtrim(rtrim(strftime('%Y-%m-%d %H:%M:%f', ""o"".""OrderDate"", CAST(10000 / 10000000.0 AS TEXT) || ' seconds'), '0'), '.') AS ""OrderDate""
FROM ""Orders"" AS ""o""
WHERE ""o"".""OrderDate"" IS NOT NULL");
        }

        public override async Task Select_expression_date_add_milliseconds_above_the_range()
        {
            await base.Select_expression_date_add_milliseconds_above_the_range();

            AssertSql(
                @"SELECT rtrim(rtrim(strftime('%Y-%m-%d %H:%M:%f', ""o"".""OrderDate"", CAST(1000000000000.0 / 1000 AS TEXT) || ' seconds'), '0'), '.') AS ""OrderDate""
FROM ""Orders"" AS ""o""
WHERE ""o"".""OrderDate"" IS NOT NULL");
        }

        public override async Task Select_expression_date_add_milliseconds_below_the_range()
        {
            await base.Select_expression_date_add_milliseconds_below_the_range();

            AssertSql(
                @"SELECT rtrim(rtrim(strftime('%Y-%m-%d %H:%M:%f', ""o"".""OrderDate"", CAST(-1000000000000.0 / 1000 AS TEXT) || ' seconds'), '0'), '.') AS ""OrderDate""
FROM ""Orders"" AS ""o""
WHERE ""o"".""OrderDate"" IS NOT NULL");
        }

        public override async Task Select_expression_date_add_milliseconds_large_number_divided()
        {
            await base.Select_expression_date_add_milliseconds_large_number_divided();

            AssertSql(
                @"@__millisecondsPerDay_0='86400000' (DbType = String)

SELECT rtrim(rtrim(strftime('%Y-%m-%d %H:%M:%f', rtrim(rtrim(strftime('%Y-%m-%d %H:%M:%f', ""o"".""OrderDate"", CAST(((CAST(strftime('%f', ""o"".""OrderDate"") AS REAL) * 1000) % 1000) / @__millisecondsPerDay_0 AS TEXT) || ' days'), '0'), '.'), CAST((((CAST(strftime('%f', ""o"".""OrderDate"") AS REAL) * 1000) % 1000) % @__millisecondsPerDay_0) / 1000 AS TEXT) || ' seconds'), '0'), '.') AS ""OrderDate""
FROM ""Orders"" AS ""o""
WHERE ""o"".""OrderDate"" IS NOT NULL");
        }

        private void AssertSql(params string[] expected)
            => Fixture.TestSqlLoggerFactory.AssertBaseline(expected);
    }
}
