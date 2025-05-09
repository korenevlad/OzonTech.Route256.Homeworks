﻿using System.Text.Json;
using System.Text.Json.Serialization;

using KafkaHomework.OrderEventGenerator;
using KafkaHomework.OrderEventGenerator.Contracts;
using KafkaHomework.OrderEventGenerator.Kafka;

const string bootstrapServers = "kafka:9092";
const string topicName = "order_events";
const int eventsCount = 100;
const int timeoutMs = 5 * 60 * 1000;

using var cts = new CancellationTokenSource(timeoutMs);
var publisher = new KafkaPublisher<long, OrderEvent>(
    bootstrapServers,
    topicName,
    keySerializer: new SystemTextJsonSerializer<long>(new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } }),
    new SystemTextJsonSerializer<OrderEvent>(new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } }));

var generator = new OrderEventGenerator();

var messages = generator
    .GenerateEvents(eventsCount)
    .Select(e => (e.OrderId, e));

await publisher.Publish(messages, cts.Token);
