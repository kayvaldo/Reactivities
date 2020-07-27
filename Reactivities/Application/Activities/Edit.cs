using System;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities
{
    public class Edit
    {
        public class Command : IRequest
        {
            public Command(Guid id, Activity activity)
            {
                Id = id;
                Activity = activity;
            }

            public Activity Activity { get; }
            public Guid Id { get; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private const string UnableToUpdateEntity = "Error occurred: Unable to update entity";

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

                    activity.Title = request.Activity.Title ?? activity.Title;
                    activity.Description = request.Activity.Description ?? activity.Description;
                    activity.Category = request.Activity.Category ?? activity.Category;
                    activity.City = request.Activity.City ?? activity.City;
                    activity.Venue = request.Activity.Venue ?? activity.Venue;
                    activity.Date = request.Activity.Date ?? activity.Date;

                    _context.Entry(activity).State = EntityState.Modified;
                    var response = await _context.SaveChangesAsync(cancellationToken);
                    return response > 0 ? Unit.Value : throw new InvalidOperationException(UnableToUpdateEntity);
                }
                catch (Exception exception)
                {
                    throw new InvalidOperationException(UnableToUpdateEntity, exception);
                }
            }
        }
    }
}