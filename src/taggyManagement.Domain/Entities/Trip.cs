using System;
using taggyManagement.Domain.Common;
using taggyManagement.Domain.ValueObjects;

namespace taggyManagement.Domain.Entities
{
    public class Trip : AggregateRoot
    {
        public Guid Id { get; private set; }
        public Guid VehicleId { get; private set; }
        public Guid? TagId { get; private set; }
        public Guid? OriginTollPlazaId { get; private set; }
        public Guid? DestinationTollPlazaId { get; private set; }
        public DateTime StartedAt { get; private set; }
        public DateTime? CompletedAt { get; private set; }
        public TripStatus Status { get; private set; }

        public Trip(Guid vehicleId)
        {
            Id = Guid.NewGuid();
            VehicleId = vehicleId;
            Status = TripStatus.Draft;
        }

        public Result<TripStatus> Start(Guid tagId, Guid? originTollPlazaId = null)
        {
            if (Status != TripStatus.Draft) return Result<TripStatus>.Fail("Trip is not in draft state");

            TagId = tagId;
            OriginTollPlazaId = originTollPlazaId;
            StartedAt = DateTime.UtcNow;
            Status = TripStatus.InProgress;
            return Result<TripStatus>.Ok(Status);
        }

        public Result<TripStatus> Complete(Guid? destinationTollPlazaId = null)
        {
            if (Status != TripStatus.InProgress) return Result<TripStatus>.Fail("Trip is not in progress");

            DestinationTollPlazaId = destinationTollPlazaId;
            CompletedAt = DateTime.UtcNow;
            Status = TripStatus.Completed;
            return Result<TripStatus>.Ok(Status);
        }

        public Result<TripStatus> Cancel()
        {
            if (Status == TripStatus.Completed) return Result<TripStatus>.Fail("Completed trips cannot be cancelled");

            Status = TripStatus.Cancelled;
            return Result<TripStatus>.Ok(Status);
        }
    }
}
