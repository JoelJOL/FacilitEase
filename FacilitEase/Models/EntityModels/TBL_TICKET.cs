using System.ComponentModel.DataAnnotations;

namespace FacilitEase.Models.EntityModels
{
    public class TBL_TICKET
    {
        [Key]
        public int Id { get; set; }
        public string TicketName { get; set; }
        public string? TicketDescription { get; set; }
        public DateTime SubmittedDate { get; set; }
        public int PriorityId { get; set; }
        private bool _isSubscribing;
        private int _statusId;
        public int StatusId
        {
            get => _statusId;
            set
            {
                if (_statusId != value)
                {
                    _statusId = value;
                    if (!_isSubscribing)
                    {
                        OnStatusChanged();
                    }
                }
            }
        }
        public int UserId { get; set; }
        public int CategoryId { get; set; }
        public int? AssignedTo { get; set; }
        public int? ControllerId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime EscalationTime { get; set; }

        // Event to be triggered when the status changes
        public event EventHandler StatusChanged;
        internal virtual void OnStatusChanged()
        {
            StatusChanged?.Invoke(this, EventArgs.Empty);
        }
        public void SubscribeToStatusChanged(EventHandler handler)
        {
            _isSubscribing = true;
            StatusChanged += handler;
            _isSubscribing = false;
        }
    }
}