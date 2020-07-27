using System;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using MediatR;
using Persistence;

namespace Application.Activities
{
    public class Details
    {
        public class Query : IRequest<Activity>
        {
            public Query(Guid id)
            {
                this.Id = id;
            }

            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Activity>
        {
            public Handler(DataContext context)
            {
                _context = context;
            }

            private readonly DataContext _context;

            public async Task<Activity> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.Activities.FindAsync(
                    new object[]
                    {
                        request.Id
                    },
                    cancellationToken);
            }
        }
    }
}