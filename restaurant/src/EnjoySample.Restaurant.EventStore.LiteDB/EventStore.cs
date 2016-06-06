using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EnjoyCQRS.Collections;
using EnjoyCQRS.Events;
using EnjoyCQRS.EventSource.Snapshots;
using EnjoyCQRS.EventSource.Storage;
using LiteDB;

namespace EnjoySample.Restaurant.EventStore.LiteDB
{
    public class EventStore : IEventStore
    {
        private readonly LiteDatabase _liteDatabase;
        private readonly LiteCollection<SnapshotData> _snapshotCollection;
        private readonly LiteCollection<EventData> _eventCollection;
        
        public EventStore(LiteDatabase liteDatabase)
        {
            _liteDatabase = liteDatabase;

            _snapshotCollection = _liteDatabase.GetCollection<SnapshotData>("snapshots");
            _eventCollection = _liteDatabase.GetCollection<EventData>("events");
        }

        public Task SaveSnapshotAsync<TSnapshot>(TSnapshot snapshot) where TSnapshot : ISnapshot
        {
            _snapshotCollection.Insert(new SnapshotData
            {
                AggregateId = snapshot.AggregateId,
                Payload = snapshot
            });

            return Task.CompletedTask;
        }

        public Task<ISnapshot> GetSnapshotByIdAsync(Guid aggregateId)
        {
            var snapshotData = _snapshotCollection.FindOne(e => e.AggregateId == aggregateId);

            return Task.FromResult(snapshotData.Payload as ISnapshot);
        }

        public Task<IEnumerable<IDomainEvent>> GetEventsForwardAsync(Guid aggregateId, int version)
        {
            var events =
                _eventCollection.Find(e => e.AggregateId == aggregateId && e.Version > version)
                    .Select(e => e.Payload)
                    .Cast<IDomainEvent>();

            return Task.FromResult(events);
        }

        public void Dispose()
        {
        }

        public void BeginTransaction()
        {
            _liteDatabase.BeginTrans();
        }

        public Task CommitAsync()
        {
            _liteDatabase.Commit();

            return Task.CompletedTask;
        }

        public void Rollback()
        {
            _liteDatabase.Rollback();
        }

        public Task<IEnumerable<IDomainEvent>> GetAllEventsAsync(Guid id)
        {
            var events = _eventCollection.Find(e => e.AggregateId == id).Select(e => e.Payload).Cast<IDomainEvent>();

            return Task.FromResult(events);
        }

        public Task SaveAsync(UncommitedDomainEventCollection collection)
        {
            foreach (var @event in collection)
            {
                _eventCollection.Insert(new EventData
                {
                    AggregateId = @event.AggregateId,
                    Payload = @event,
                    Id = @event.Id,
                    Version = @event.Version
                });
            }

            return Task.CompletedTask;
        }
    }

    public class SnapshotData
    {
        public Guid AggregateId { get; set; }
        public object Payload { get; set; }
    }

    public class EventData
    {
        public Guid Id { get; set; }
        public Guid AggregateId { get; set; }
        public int Version { get; set; }
        public object Payload { get; set; }
        public Dictionary<string, object> Metadata { get; set; } = new Dictionary<string, object>();
    }
}
