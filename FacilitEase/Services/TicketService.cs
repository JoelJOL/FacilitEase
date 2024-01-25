using FacilitEase.Data;
using FacilitEase.Models.ApiModels;
using FacilitEase.Models.EntityModels;
using FacilitEase.Repositories;
using FacilitEase.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace FacilitEase.Services
{
    public class TicketService : ITicketService
    {
        private readonly AppDbContext _dbContext;
 
        private readonly ITicketRepository _ticketRepository;
        private readonly IDocumentRepository _documentRepository;
        public TicketService(AppDbContext dbContext, ITicketRepository ticketRepository, IDocumentRepository documentRepository)
        {
            _dbContext = dbContext;
            _ticketRepository = ticketRepository;
            _documentRepository = documentRepository;
        }

        public void CreateTicketWithDocuments(TicketDto ticketDto)
        {
            
                    
                    var ticketEntity = new TBL_TICKET
                    {
                        TicketName = ticketDto.TicketName,
                        TicketDescription = ticketDto.TicketDescription,
                        PriorityId = ticketDto.PriorityId,
                        CategoryId = ticketDto.CategoryId,
                    };
            _dbContext.Add(ticketEntity);

                    _dbContext.SaveChanges();

                    
                    foreach (var documentLink in ticketDto.DocumentLink)
                    {
                        var documentEntity = new TBL_DOCUMENT
                        {
                            DocumentLink = documentLink,
                            TicketId = ticketEntity.Id ,
                        };

                        _documentRepository.Add(documentEntity);
                    }

                    _dbContext.SaveChanges();

            }
        }
    }



