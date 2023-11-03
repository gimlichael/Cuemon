using System;
using System.Data;
using System.Reflection;
using Cuemon.Collections.Generic;
using Microsoft.Data.Sqlite;

namespace Cuemon.Data.Assets
{
    internal class FakeDataManager(Action<DataManagerOptions> setup) : DataManager(setup)
    {
        public override DataManager Clone()
        {
            return new FakeDataManager(Patterns.ConfigureRevert(Options));
        }

        protected override IDbCommand GetDbCommand(DataStatement statement)
        {
            return Patterns.SafeInvoke(() => new SqliteCommand(statement.Text, new SqliteConnection(Options.ConnectionString)), sc =>
            {
                foreach (var parameter in statement.Parameters)
                {
                    sc.Parameters.Add(parameter);
                }
                sc.CommandTimeout = (int)statement.Timeout.TotalSeconds;
                sc.CommandType = statement.Type;
                return sc;
            }, ex => throw ExceptionInsights.Embed(new InvalidOperationException("There is an error when creating a new SqlCommand.", ex), MethodBase.GetCurrentMethod(), Arguments.ToArray(statement)));
        }
    }
}
