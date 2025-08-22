namespace CustomerService.Domain.Enums
{
    public enum CustomerStatus
    {
        Pending = 0,
        CreditProposalRequested = 1,
        CreditProposalApproved = 2,
        CreditProposalRejected = 3,
        CardRequested = 4,
        CardIssued = 5,
        Active = 6,
        Inactive = 7
    }
}