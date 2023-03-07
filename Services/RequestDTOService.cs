﻿using Entities.Models;
using Repository.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services;

public static class RequestDTOService
{
    public static RequestDTO GetDtoFromRequest(Request request)
    {
        RequestDTO returnedDto = new RequestDTO
        {
            Id = request.Id.ToString(),
            Description = request.Description,
            Justification = request.Justification,
            RejectionReason = request.RejectionReason.ToString(),
            DeliveryMode = request.DeliveryMode,
            SubmittedDate = request.SubmittedDate,
            DateNeeded = request.DateNeeded.ToString(),
            Status = request.Status,
            Total = request.Total.ToString(),
            UserId = request.UserId.ToString()
        };

        return returnedDto;
    }

    public static List<RequestDTO> GetDtosFromRequests(List<Request> requests)
    {

        List<RequestDTO> requestsDto = new List<RequestDTO>();

        foreach (Request request in requests)
        {
            requestsDto.Add(GetDtoFromRequest(request));
        }

        return requestsDto;
    }
}
