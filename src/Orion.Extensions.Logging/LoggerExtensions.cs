using System;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;
using Orion.Extensions.Logging.States;

namespace Orion.Extensions.Logging {
    public static class LoggerExtensions {
        #region Http
        public static void LogHttpRequest(this ILogger logger,
            string url,
            string httpMethod,
            DateTimeOffset startTime,
            TimeSpan duration,
            LogLevel logLevel = LogLevel.Information,
            string responseCode = null,
            string operationSystem = null) {
            var state = new HttpRequestState {
                HttpMethod = httpMethod,
                EventTime = startTime,
                RequestUrl = url,
                ResponseCode = responseCode,
                ResponseTime = duration,
                OperationSystem = operationSystem ?? RuntimeInformation.OSDescription,
                Success = true
            };
            logger.Log(logLevel, 1, state, null, HttpRequestState.Formatter);
        }

        public static void LogHttpRequest(this ILogger logger,
            string url,
            string httpMethod,
            DateTimeOffset startTime,
            TimeSpan duration,
            Exception exception,
            LogLevel logLevel = LogLevel.Error,
            string operationSystem = null) {
            var state = new HttpRequestState {
                HttpMethod = httpMethod,
                EventTime = startTime,
                RequestUrl = url,
                ResponseTime = duration,
                OperationSystem = operationSystem ?? RuntimeInformation.OSDescription,
                Success = exception == null
            };
            logger.Log(logLevel, 1, state, exception, HttpRequestState.Formatter);
        }
        #endregion

        #region Event

        public static void LogEvent(this ILogger logger, 
            string eventName, 
            DateTimeOffset? eventDateTime = null, 
            LogLevel logLevel = LogLevel.Information) {
            logger.Log(logLevel, 1, new EventState {
                EventTime = eventDateTime ?? DateTimeOffset.UtcNow,
                Name = eventName
            }, null, EventState.Formatter);
        }

        #endregion

        #region Metric

        public static void LogMetric(this ILogger logger, 
            string metricName, 
            double value,
            DateTimeOffset? eventDateTime = null,
            LogLevel logLevel = LogLevel.Information) {
            logger.Log(logLevel, 1, new MetricState {
                EventTime = eventDateTime ?? DateTimeOffset.UtcNow,
                Name = metricName,
                Value = value
            }, null, MetricState.Formatter);
        }
        #endregion

        #region Sql

        public static void LogSql(this ILogger logger,
            string serverName,
            string sqlRequest,
            TimeSpan duration,
            DateTimeOffset startTime,
            bool success = true,
            LogLevel logLevel = LogLevel.Information) {
            var state = new SqlDependencyState {
                EventTime = startTime,
                Request = sqlRequest,
                ResponseTime = duration,
                Success = success,
                ServerName = serverName
            };
            logger.Log(logLevel, 1, state, null, SqlDependencyState.Formatter);
        }

        public static void LogSql(this ILogger logger,
            string serverName,
            string sqlRequest,
            TimeSpan duration,
            DateTimeOffset startTime,
            Exception exception,
            LogLevel logLevel = LogLevel.Error)
        {
            var state = new SqlDependencyState
            {
                EventTime = startTime,
                Request = sqlRequest,
                ResponseTime = duration,
                Success = false,
                ServerName = serverName
            };
            logger.Log(logLevel, 1, state, exception, SqlDependencyState.Formatter);
        }
        #endregion
    }
}