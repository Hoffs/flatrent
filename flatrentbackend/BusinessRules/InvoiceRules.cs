using System;
using System.Collections.Generic;
using System.Linq;
using FlatRent.Constants;
using FlatRent.Entities;
using FlatRent.Extensions;
using FlatRent.Models;

namespace FlatRent.BusinessRules
{
    public static class InvoiceRules
    {
        public static (bool, FormError) AddPreviousInvoiceIfNotPaid(Invoice invoice, Invoice lastInvoice, Agreement agreement)
        {
            if (!lastInvoice.IsPaid)
            {
                lastInvoice.IsValid = false;
                lastInvoice.Incidents.SetProperty(i => i.Invoice, invoice);

                invoice.AmountToPay += lastInvoice.AmountToPay;
                return (true, null);
            }

            return (true, null);
        }

        public static (bool, FormError) CalculatePriceForPeriodIfShorterThan30(Invoice invoice, Agreement agreement)
        {
            if (invoice.InvoicedPeriodTo > agreement.To)
            {
                invoice.InvoicedPeriodTo = agreement.To.Date;
                invoice.DueDate = agreement.To.Date;

                var daysLeft = invoice.InvoicedPeriodTo.Date.Subtract(invoice.InvoicedPeriodFrom.Date).Days;
                invoice.AmountToPay += (float)Math.Round(agreement.Price * ((float)daysLeft / BusinessConstants.RentMonthInDays), 2, MidpointRounding.AwayFromZero);
            }
            else
            {
                invoice.AmountToPay += agreement.Price;
            }

            return (true, null);
        }

        public static (bool, FormError) AddEachNotInvoicedRepairedIncident(Invoice invoice, Agreement agreement)
        {
            var notInvoicedIncidents = agreement.Incidents.Where(Incident.NotInvoicedIncidentsFunc).ToList();
            if (notInvoicedIncidents.Count > 0)
            {
                if (invoice.Incidents == null) invoice.Incidents = new List<Incident>();

                notInvoicedIncidents.SetProperty(i => i.Invoice, invoice);
                invoice.AmountToPay += notInvoicedIncidents.Sum(i => i.Price);
            }

            return (true, null);
        }
    }
}