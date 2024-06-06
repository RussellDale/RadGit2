using Rad2.Models.Domian;
using GridCore.Server;
using GridMvc.Server;
using GridShared;
using GridShared.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Immutable;

namespace Rad2.Services
{
    public class OfficeAssignmentService : IOfficeAssignmentService
    {
        private readonly DbContextOptions<dbContext> _options;

        public OfficeAssignmentService(DbContextOptions<dbContext> options)
        {
            _options = options;
        }
        public ItemsDTO<OfficeAssignment> GetOfficeAssignmentGridRow(Action<IGridColumnCollection<OfficeAssignment>> columns,
                                                                     QueryDictionary<StringValues> query, bool isName, string name)
        {
            using (var context = new dbContext(_options))
            {
                var repository = new OfficeAssignmentRepository(context);
                
                IQueryable<OfficeAssignment> officeAssignment = isName == true ? 
                 repository.GetAll().Where(c => c.Instructor.FirstName + " " + c.Instructor.LastName == name ) : 
                 repository.GetAll();
                
                var server = new GridServer<OfficeAssignment>(officeAssignment, new QueryCollection(query),
                        false, "OfficeAssignmentGrid", columns)
                            .WithPaging(10)
                            .Sortable()
                            .Searchable(true)
                    ;

                var items = server.ItemsToDisplay;
                return items;
            }
        }
        public IEnumerable<SelectItem> GetAllOfficeAssignment()
        {
            using (var context = new dbContext(_options))
            {
                InstructorRepository repository = new InstructorRepository(context);

                return repository.GetAll().Where(x => x.OfficeAssignment.InstructorId != x.Id)
                     .Select(r => new SelectItem(r.Id.ToString(), r.Id.ToString() + " - "
                       + r.FirstName + " " + r.LastName))
                    .ToList();
            }
        }

        public async Task<OfficeAssignment> Get(params object[] keys)
        {
            using (var context = new dbContext(_options))
            {
                int OfficeAssignmentID;
                int.TryParse(keys[0].ToString(), out OfficeAssignmentID);
                var repository = new OfficeAssignmentRepository(context);
                return await repository.GetById(OfficeAssignmentID);
            }
        }

        public async Task Insert(OfficeAssignment item)
        {
            using (var context = new dbContext(_options))
            {
                try
                {
                    var repository = new OfficeAssignmentRepository(context);
                    await repository.Insert(item);
                    repository.Save();
                }
                catch (Exception e)
                {
                    throw new GridException("DETSRV-01", e);
                }
            }
        }

        public async Task Update(OfficeAssignment item)
        {
            using (var context = new dbContext(_options))
            {
                try
                {
                    var repository = new OfficeAssignmentRepository(context);
                    await repository.Update(item);
                    repository.Save();
                }
                catch (Exception e)
                {
                    throw new GridException(e);
                }
            }
        }

        public async Task Delete(params object[] keys)
        {
            using (var context = new dbContext(_options))
            {
                try
                {
                    var OfficeAssignment = await Get(keys);
                    var repository = new OfficeAssignmentRepository(context);
                    repository.Delete(OfficeAssignment);
                    repository.Save();
                }
                catch (Exception)
                {
                    throw new GridException("Error deleting the office assignment");
                }
            }
        }
    }

    public interface IOfficeAssignmentService : ICrudDataService<OfficeAssignment>
    {
        ItemsDTO<OfficeAssignment> GetOfficeAssignmentGridRow(Action<IGridColumnCollection<OfficeAssignment>> columns,
                                                              QueryDictionary<StringValues> query, bool isName, string name);
        IEnumerable<SelectItem> GetAllOfficeAssignment();
    }
}