<template>
  <div class="admin-layout">
    <AdminSidebar />
    <div class="payment-management">
      <div class="page-header">
        <div class="header-content">
          <h1>Payment Management</h1>
          <p class="subtitle">Manage and track all payment transactions</p>
        </div>
        <div class="header-actions">
          <div class="search-box">
            <i class="fas fa-search search-icon"></i>
            <input 
              type="text" 
              v-model="searchQuery" 
              placeholder="Search by order ID or customer name..."
              @input="handleSearch"
            >
          </div>
          <div class="filter-section">
            <select v-model="statusFilter" @change="handleFilter">
              <option value="">All Status</option>
              <option value="pending">Pending</option>
              <option value="completed">Completed</option>
              <option value="failed">Failed</option>
              <option value="refunded">Refunded</option>
            </select>
            <i class="fas fa-chevron-down select-icon"></i>
          </div>
        </div>
      </div>

      <div class="stats-cards">
        <div class="stat-card">
          <div class="stat-icon completed">
            <i class="fas fa-check-circle"></i>
          </div>
          <div class="stat-info">
            <h3>Completed</h3>
            <p class="stat-value">$12,450</p>
            <p class="stat-change positive">+12.5%</p>
          </div>
        </div>
        <div class="stat-card">
          <div class="stat-icon pending">
            <i class="fas fa-clock"></i>
          </div>
          <div class="stat-info">
            <h3>Pending</h3>
            <p class="stat-value">$3,250</p>
            <p class="stat-change">5 transactions</p>
          </div>
        </div>
        <div class="stat-card">
          <div class="stat-icon failed">
            <i class="fas fa-times-circle"></i>
          </div>
          <div class="stat-info">
            <h3>Failed</h3>
            <p class="stat-value">$850</p>
            <p class="stat-change negative">-2.3%</p>
          </div>
        </div>
        <div class="stat-card">
          <div class="stat-icon refunded">
            <i class="fas fa-undo"></i>
          </div>
          <div class="stat-info">
            <h3>Refunded</h3>
            <p class="stat-value">$1,200</p>
            <p class="stat-change">3 transactions</p>
          </div>
        </div>
      </div>

      <div class="payment-table-container">
        <div class="table-header">
          <h2>Recent Transactions</h2>
          <div class="table-actions">
            <button class="export-btn">
              <i class="fas fa-download"></i>
              Export
            </button>
          </div>
        </div>
        <div class="payment-table">
          <table>
            <thead>
              <tr>
                <th>Order ID</th>
                <th>Customer</th>
                <th>Amount</th>
                <th>Payment Method</th>
                <th>Date</th>
                <th>Status</th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="payment in filteredPayments" :key="payment.id" class="table-row">
                <td>
                  <span class="order-id">{{ payment.orderId }}</span>
                </td>
                <td>
                  <div class="customer-info">
                    <div class="customer-avatar">
                      {{ getInitials(payment.customerName) }}
                    </div>
                    <span>{{ payment.customerName }}</span>
                  </div>
                </td>
                <td>
                  <span class="amount">{{ formatCurrency(payment.amount) }}</span>
                </td>
                <td>
                  <div class="payment-method">
                    <i :class="getPaymentMethodIcon(payment.paymentMethod)"></i>
                    <span>{{ payment.paymentMethod }}</span>
                  </div>
                </td>
                <td>
                  <span class="date">{{ formatDate(payment.date) }}</span>
                </td>
                <td>
                  <span :class="['status-badge', payment.status.toLowerCase()]">
                    {{ payment.status }}
                  </span>
                </td>
                <td>
                  <div class="action-buttons">
                    <button 
                      class="action-btn view-btn"
                      @click="viewPaymentDetails(payment)"
                      title="View Details"
                    >
                      <i class="fas fa-eye"></i>
                    </button>
                    <button 
                      v-if="payment.status === 'PENDING'"
                      class="action-btn approve-btn"
                      @click="approvePayment(payment)"
                      title="Approve Payment"
                    >
                      <i class="fas fa-check"></i>
                    </button>
                    <button 
                      v-if="payment.status === 'COMPLETED'"
                      class="action-btn refund-btn"
                      @click="refundPayment(payment)"
                      title="Refund Payment"
                    >
                      <i class="fas fa-undo"></i>
                    </button>
                  </div>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>

      <!-- Payment Details Modal -->
      <div v-if="showModal" class="modal-overlay" @click="closeModal">
        <div class="modal-content" @click.stop>
          <div class="modal-header">
            <h2>Payment Details</h2>
            <button @click="closeModal" class="close-btn">
              <i class="fas fa-times"></i>
            </button>
          </div>
          <div class="modal-body" v-if="selectedPayment">
            <div class="detail-section">
              <h3>Transaction Information</h3>
              <div class="detail-grid">
                <div class="detail-item">
                  <span class="label">Order ID</span>
                  <span class="value">{{ selectedPayment.orderId }}</span>
                </div>
                <div class="detail-item">
                  <span class="label">Transaction ID</span>
                  <span class="value">{{ selectedPayment.transactionId }}</span>
                </div>
                <div class="detail-item">
                  <span class="label">Amount</span>
                  <span class="value amount">{{ formatCurrency(selectedPayment.amount) }}</span>
                </div>
                <div class="detail-item">
                  <span class="label">Status</span>
                  <span :class="['status-badge', selectedPayment.status.toLowerCase()]">
                    {{ selectedPayment.status }}
                  </span>
                </div>
              </div>
            </div>
            
            <div class="detail-section">
              <h3>Customer Information</h3>
              <div class="detail-grid">
                <div class="detail-item">
                  <span class="label">Customer Name</span>
                  <span class="value">{{ selectedPayment.customerName }}</span>
                </div>
                <div class="detail-item">
                  <span class="label">Payment Method</span>
                  <div class="payment-method">
                    <i :class="getPaymentMethodIcon(selectedPayment.paymentMethod)"></i>
                    <span>{{ selectedPayment.paymentMethod }}</span>
                  </div>
                </div>
                <div class="detail-item">
                  <span class="label">Date</span>
                  <span class="value">{{ formatDate(selectedPayment.date) }}</span>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: 'ManagePayment',
  data() {
    return {
      searchQuery: '',
      statusFilter: '',
      showModal: false,
      selectedPayment: null,
      payments: [
        {
          id: 1,
          orderId: 'ORD-001',
          customerName: 'John Doe',
          amount: 150.00,
          paymentMethod: 'Credit Card',
          date: '2024-03-15',
          status: 'COMPLETED',
          transactionId: 'TRX-123456'
        },
        {
          id: 2,
          orderId: 'ORD-002',
          customerName: 'Jane Smith',
          amount: 75.50,
          paymentMethod: 'PayPal',
          date: '2024-03-14',
          status: 'PENDING',
          transactionId: 'TRX-123457'
        },
        {
          id: 3,
          orderId: 'ORD-003',
          customerName: 'Mike Johnson',
          amount: 299.99,
          paymentMethod: 'Bank Transfer',
          date: '2024-03-13',
          status: 'FAILED',
          transactionId: 'TRX-123458'
        },
        {
          id: 4,
          orderId: 'ORD-004',
          customerName: 'Sarah Wilson',
          amount: 199.50,
          paymentMethod: 'Credit Card',
          date: '2024-03-12',
          status: 'REFUNDED',
          transactionId: 'TRX-123459'
        }
      ]
    }
  },
  computed: {
    filteredPayments() {
      return this.payments.filter(payment => {
        const matchesSearch = 
          payment.orderId.toLowerCase().includes(this.searchQuery.toLowerCase()) ||
          payment.customerName.toLowerCase().includes(this.searchQuery.toLowerCase())
        
        const matchesStatus = !this.statusFilter || 
          payment.status.toLowerCase() === this.statusFilter.toLowerCase()
        
        return matchesSearch && matchesStatus
      })
    }
  },
  methods: {
    formatCurrency(amount) {
      return new Intl.NumberFormat('en-US', {
        style: 'currency',
        currency: 'USD'
      }).format(amount)
    },
    formatDate(date) {
      return new Date(date).toLocaleDateString('en-US', {
        year: 'numeric',
        month: 'short',
        day: 'numeric'
      })
    },
    getInitials(name) {
      return name
        .split(' ')
        .map(word => word[0])
        .join('')
        .toUpperCase()
    },
    getPaymentMethodIcon(method) {
      const icons = {
        'Credit Card': 'fas fa-credit-card',
        'PayPal': 'fab fa-paypal',
        'Bank Transfer': 'fas fa-university'
      }
      return icons[method] || 'fas fa-money-bill'
    },
    handleSearch() {
      // Implement search logic
    },
    handleFilter() {
      // Implement filter logic
    },
    viewPaymentDetails(payment) {
      this.selectedPayment = payment
      this.showModal = true
    },
    closeModal() {
      this.showModal = false
      this.selectedPayment = null
    },
    async approvePayment(payment) {
      try {
        // Implement payment approval logic
        // await this.$axios.post(`/api/payments/${payment.id}/approve`)
        this.$toast.success('Payment approved successfully')
      } catch (error) {
        this.$toast.error('Failed to approve payment')
      }
    },
    async refundPayment(payment) {
      try {
        // Implement refund logic
        // await this.$axios.post(`/api/payments/${payment.id}/refund`)
        this.$toast.success('Refund processed successfully')
      } catch (error) {
        this.$toast.error('Failed to process refund')
      }
    }
  }
}
</script>

<style scoped>
.admin-layout {
  display: flex;
  min-height: 100vh;
  background-color: #f8f9fa;
}

.payment-management {
  flex: 1;
  padding: 24px;
  margin-left: 280px; /* Width of the sidebar */
  min-height: 100vh;
  background-color: #f8f9fa;
  transition: margin-left 0.3s ease;
}

/* Header Styles */
.page-header {
  background: white;
  padding: 24px;
  border-radius: 12px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.05);
  margin-bottom: 24px;
  margin-right: 24px; /* Add right margin to prevent content from touching the edge */
}

.header-content h1 {
  font-size: 24px;
  color: #1a1f36;
  margin: 0;
  font-weight: 600;
}

.subtitle {
  color: #697386;
  margin: 4px 0 0;
  font-size: 14px;
}

.header-actions {
  display: flex;
  gap: 16px;
  margin-top: 20px;
}

/* Search Box Styles */
.search-box {
  position: relative;
  flex: 1;
  max-width: 400px;
}

.search-box input {
  width: 100%;
  padding: 12px 40px 12px 16px;
  border: 1px solid #e3e8ef;
  border-radius: 8px;
  font-size: 14px;
  transition: all 0.2s ease;
  background-color: #f8fafc;
}

.search-box input:focus {
  outline: none;
  border-color: #4f46e5;
  background-color: white;
  box-shadow: 0 0 0 3px rgba(79, 70, 229, 0.1);
}

.search-icon {
  position: absolute;
  right: 12px;
  top: 50%;
  transform: translateY(-50%);
  color: #697386;
}

/* Filter Section Styles */
.filter-section {
  position: relative;
}

.filter-section select {
  appearance: none;
  padding: 12px 40px 12px 16px;
  border: 1px solid #e3e8ef;
  border-radius: 8px;
  font-size: 14px;
  background-color: #f8fafc;
  cursor: pointer;
  min-width: 160px;
  transition: all 0.2s ease;
}

.filter-section select:focus {
  outline: none;
  border-color: #4f46e5;
  background-color: white;
  box-shadow: 0 0 0 3px rgba(79, 70, 229, 0.1);
}

.select-icon {
  position: absolute;
  right: 12px;
  top: 50%;
  transform: translateY(-50%);
  color: #697386;
  pointer-events: none;
}

/* Stats Cards Styles */
.stats-cards {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(240px, 1fr));
  gap: 24px;
  margin-bottom: 24px;
}

.stat-card {
  background: white;
  padding: 24px;
  border-radius: 12px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.05);
  display: flex;
  align-items: center;
  gap: 16px;
  transition: transform 0.2s ease;
}

.stat-card:hover {
  transform: translateY(-2px);
}

.stat-icon {
  width: 48px;
  height: 48px;
  border-radius: 12px;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 24px;
}

.stat-icon.completed {
  background-color: #dcfce7;
  color: #16a34a;
}

.stat-icon.pending {
  background-color: #fef9c3;
  color: #ca8a04;
}

.stat-icon.failed {
  background-color: #fee2e2;
  color: #dc2626;
}

.stat-icon.refunded {
  background-color: #e0e7ff;
  color: #4f46e5;
}

.stat-info h3 {
  margin: 0;
  font-size: 14px;
  color: #697386;
  font-weight: 500;
}

.stat-value {
  margin: 4px 0;
  font-size: 24px;
  font-weight: 600;
  color: #1a1f36;
}

.stat-change {
  font-size: 12px;
  color: #697386;
}

.stat-change.positive {
  color: #16a34a;
}

.stat-change.negative {
  color: #dc2626;
}

/* Table Container Styles */
.payment-table-container {
  background: white;
  border-radius: 12px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.05);
  overflow: hidden;
  margin-right: 24px;
}

.table-header {
  padding: 20px 24px;
  border-bottom: 1px solid #e3e8ef;
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.table-header h2 {
  margin: 0;
  font-size: 18px;
  color: #1a1f36;
  font-weight: 600;
}

.export-btn {
  display: flex;
  align-items: center;
  gap: 8px;
  padding: 8px 16px;
  background-color: #f8fafc;
  border: 1px solid #e3e8ef;
  border-radius: 6px;
  color: #1a1f36;
  font-size: 14px;
  cursor: pointer;
  transition: all 0.2s ease;
}

.export-btn:hover {
  background-color: #f1f5f9;
  border-color: #cbd5e1;
}

/* Table Styles */
.payment-table {
  overflow-x: auto;
}

table {
  width: 100%;
  border-collapse: collapse;
}

th {
  background-color: #f8fafc;
  padding: 16px 24px;
  text-align: left;
  font-weight: 500;
  color: #697386;
  font-size: 14px;
  white-space: nowrap;
}

td {
  padding: 16px 24px;
  border-bottom: 1px solid #e3e8ef;
  color: #1a1f36;
  font-size: 14px;
}

.table-row:hover {
  background-color: #f8fafc;
}

/* Customer Info Styles */
.customer-info {
  display: flex;
  align-items: center;
  gap: 12px;
}

.customer-avatar {
  width: 32px;
  height: 32px;
  background-color: #e0e7ff;
  color: #4f46e5;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 12px;
  font-weight: 600;
}

/* Payment Method Styles */
.payment-method {
  display: flex;
  align-items: center;
  gap: 8px;
}

.payment-method i {
  color: #697386;
}

/* Status Badge Styles */
.status-badge {
  padding: 6px 12px;
  border-radius: 20px;
  font-size: 12px;
  font-weight: 500;
  display: inline-block;
}

.status-badge.completed {
  background-color: #dcfce7;
  color: #16a34a;
}

.status-badge.pending {
  background-color: #fef9c3;
  color: #ca8a04;
}

.status-badge.failed {
  background-color: #fee2e2;
  color: #dc2626;
}

.status-badge.refunded {
  background-color: #e0e7ff;
  color: #4f46e5;
}

/* Action Buttons Styles */
.action-buttons {
  display: flex;
  gap: 8px;
}

.action-btn {
  width: 32px;
  height: 32px;
  border-radius: 6px;
  border: none;
  display: flex;
  align-items: center;
  justify-content: center;
  cursor: pointer;
  transition: all 0.2s ease;
  color: white;
}

.view-btn {
  background-color: #e0e7ff;
  color: #4f46e5;
}

.view-btn:hover {
  background-color: #c7d2fe;
}

.approve-btn {
  background-color: #16a34a;
}

.approve-btn:hover {
  background-color: #15803d;
}

.refund-btn {
  background-color: #dc2626;
}

.refund-btn:hover {
  background-color: #b91c1c;
}

/* Modal Styles */
.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background-color: rgba(0, 0, 0, 0.5);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 1100; /* Higher than sidebar z-index */
  backdrop-filter: blur(4px);
}

.modal-content {
  background: white;
  border-radius: 12px;
  width: 600px;
  max-width: 90%;
  max-height: 90vh;
  overflow-y: auto;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
}

.modal-header {
  padding: 20px 24px;
  border-bottom: 1px solid #e3e8ef;
  display: flex;
  justify-content: space-between;
  align-items: center;
  position: sticky;
  top: 0;
  background: white;
  z-index: 1;
}

.modal-header h2 {
  margin: 0;
  font-size: 20px;
  color: #1a1f36;
  font-weight: 600;
}

.close-btn {
  background: none;
  border: none;
  width: 32px;
  height: 32px;
  border-radius: 6px;
  display: flex;
  align-items: center;
  justify-content: center;
  cursor: pointer;
  color: #697386;
  transition: all 0.2s ease;
}

.close-btn:hover {
  background-color: #f1f5f9;
  color: #1a1f36;
}

.modal-body {
  padding: 24px;
}

.detail-section {
  margin-bottom: 24px;
}

.detail-section h3 {
  margin: 0 0 16px;
  font-size: 16px;
  color: #1a1f36;
  font-weight: 600;
}

.detail-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
  gap: 16px;
}

.detail-item {
  display: flex;
  flex-direction: column;
  gap: 4px;
}

.detail-item .label {
  font-size: 12px;
  color: #697386;
}

.detail-item .value {
  font-size: 14px;
  color: #1a1f36;
  font-weight: 500;
}

.detail-item .value.amount {
  font-size: 16px;
  font-weight: 600;
  color: #16a34a;
}

/* Responsive Design */
@media (max-width: 1024px) {
  .payment-management {
    margin-left: 240px; /* Smaller sidebar width on medium screens */
  }
}

@media (max-width: 768px) {
  .payment-management {
    margin-left: 0; /* Remove margin when sidebar is collapsed */
    padding: 16px;
  }
  
  .page-header {
    margin-right: 16px;
  }
  
  .header-actions {
    flex-direction: column;
  }
  
  .search-box {
    max-width: none;
  }
  
  .stats-cards {
    grid-template-columns: 1fr;
  }
  
  .table-header {
    flex-direction: column;
    gap: 16px;
    align-items: flex-start;
  }
  
  .export-btn {
    width: 100%;
    justify-content: center;
  }
}

@media (max-width: 480px) {
  .payment-management {
    padding: 12px;
  }
  
  .page-header {
    padding: 16px;
    margin-right: 12px;
  }
  
  .header-content h1 {
    font-size: 20px;
  }
  
  .stat-card {
    padding: 16px;
  }
  
  .stat-value {
    font-size: 20px;
  }
  
  .modal-content {
    width: 95%;
  }
}

/* Add styles for when sidebar is collapsed */
.sidebar-collapsed .payment-management {
  margin-left: 80px; /* Width of collapsed sidebar */
}

/* Ensure modal appears above sidebar */
.modal-overlay {
  z-index: 1100; /* Higher than sidebar z-index */
}

/* Adjust table container for better scrolling */
.payment-table-container {
  margin-right: 24px;
  overflow: hidden;
}

@media (max-width: 768px) {
  .payment-table-container {
    margin-right: 16px;
  }
}

@media (max-width: 480px) {
  .payment-table-container {
    margin-right: 12px;
  }
}
</style> 