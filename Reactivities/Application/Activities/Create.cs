using System;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using MediatR;
using Persistence;

namespace Application.Activities
{
    public class Create
    {
        public class Command : IRequest
        {
            public Command(Activity activity)
            {
                Activity = activity;
            }

            public Activity Activity { get; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private const string UnableToCreateEntity = "Error occurred: Unable to create entity";

            public Handler(DataContext context)
            {
                _context = context;
            }

            private readonly DataContext _context;

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                try
                {
                    _context.Add(request.Activity);
                    var response = await _context.SaveChangesAsync(cancellationToken);
                    return response > 0 ? Unit.Value : throw new InvalidOperationException(UnableToCreateEntity);
                }
                catch (Exception exception)
                {
                    throw new InvalidOperationException(UnableToCreateEntity, exception);
                }
            }
        }
    }
}