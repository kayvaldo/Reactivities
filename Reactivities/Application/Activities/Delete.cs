using System;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using MediatR;
using Persistence;

namespace Application.Activities
{
    public class Delete
    {
        public class Command : IRequest
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private const string UnableToDeleteEntity = "Error occurred: Unable to delete entity";

            public Handler(DataContext context)
            {
                _context = context;
            }

            private readonly DataContext _context;

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                try
                {
                    var activity = await _context.FindAsync<Activity>(request.Id);

                    if (activity == null)
                    {
                        throw new InvalidOperationException("Error occurred: Entity not found");
                    }

                    _context.Remove(activity);
                    var response = await _context.SaveChangesAsync(cancellationToken);
                    return response > 0 ? Unit.Value : throw new InvalidOperationException(UnableToDeleteEntity);
                }
                catch (Exception exception)
                {
                    throw new InvalidOperationException(UnableToDeleteEntity, exception);
                }
            }
        }
    }
}