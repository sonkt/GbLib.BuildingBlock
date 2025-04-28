Module | Chức năng
BuildingBlocks.Domain | Base Entity, Event, ValueObject, Repository Interface
BuildingBlocks.Application | Base Command, Query, Handler, Validation, Exception
BuildingBlocks.Infrastructure | Base Repository EF Core, UnitOfWork, Event Dispatcher
BuildingBlocks.Persistence | Database Connection, Migration Helper
BuildingBlocks.EventBus | Gửi/nhận event giữa các service (RabbitMQ/Kafka)
BuildingBlocks.ExceptionHandling | Middleware xử lý lỗi toàn cục