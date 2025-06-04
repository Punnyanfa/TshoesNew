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
              placeholder="Search by Order ID..."
            >
          </div>
          <div class="filter-section">
            <select v-model="statusFilter">
              <option value="">All Status</option>
              <option value="PENDING">Pending</option>
              <option value="COMPLETED">Completed</option>
              <option value="FAILED">Failed</option>
              <!-- Removed Refunded as it's not in current API status values -->
            </select>
            <i class="fas fa-chevron-down select-icon"></i>
          </div>
        </div>
      </div>

      <!-- Keep stats cards or remove if data not available from API -->
      <div class="stats-cards">
        <div class="stat-card">
          <div class="stat-icon completed">
            <i class="fas fa-check-circle"></i>
          </div>
          <div class="stat-info">
            <h3>Completed</h3>
            <p class="stat-value">{{ formatCurrency(completedStats.total) }}</p>
            <p class="stat-change positive">{{ completedStats.count }} transactions</p>
          </div>
        </div>
        <div class="stat-card">
          <div class="stat-icon pending">
            <i class="fas fa-clock"></i>
          </div>
          <div class="stat-info">
            <h3>Pending</h3>
            <p class="stat-value">{{ formatCurrency(pendingStats.total) }}</p>
            <p class="stat-change">{{ pendingStats.count }} transactions</p>
          </div>
        </div>
        <div class="stat-card">
          <div class="stat-icon failed">
            <i class="fas fa-times-circle"></i>
          </div>
          <div class="stat-info">
            <h3>Failed</h3>
            <p class="stat-value">{{ formatCurrency(failedStats.total) }}</p>
            <p class="stat-change negative">{{ failedStats.count }} transactions</p>
          </div>
        </div>
        <div class="stat-card">
          <div class="stat-icon refunded">
            <i class="fas fa-undo"></i>
          </div>
          <div class="stat-info">
            <h3>Refunded</h3>
            <p class="stat-value">{{ formatCurrency(refundedStats.total) }}</p>
            <p class="stat-change">{{ refundedStats.count }} transactions</p>
          </div>
        </div>
      </div>

      <div class="payment-table-container">
        <div class="table-header">
          <h2>Recent Transactions</h2>
          <!-- Removed Export button as related action methods are removed -->
        </div>
        <div class="payment-table">
          <table>
            <thead>
              <tr>
                <th>ID</th>
                <th>Order ID</th>
                <th>Amount</th>
                <th>Payment Method</th>
                <th>Status</th>
                <!-- Removed Customer, Date, Actions columns -->
              </tr>
            </thead>
            <tbody>
              <tr v-for="payment in paginatedPayments" :key="payment.id" class="table-row">
                <td>{{ payment.id }}</td>
                <td>
                  <span class="order-id">{{ payment.orderId }}</span>
                </td>
                <td>
                  <span class="amount">{{ formatCurrency(payment.amount) }}</span>
                </td>
                <td>
                  <div class="payment-method">
                    <i :class="getPaymentMethodIcon(payment.paymentMethod)"></i>
                    <span>{{ formatPaymentMethod(payment.paymentMethod) }}</span>
                  </div>
                </td>
                <td>
                  <span :class="['status-badge', formatStatus(payment.status).toLowerCase()]">
                    {{ formatStatus(payment.status) }}
                  </span>
                </td>
                <!-- Removed Customer, Date, Actions cells -->
              </tr>
            </tbody>
          </table>
        </div>
        <!-- Add pagination -->
        <div class="pagination">
          <button 
            class="pagination-btn" 
            :disabled="currentPage === 1"
            @click="goToPage(currentPage - 1)"
          >
            <i class="fas fa-chevron-left"></i>
          </button>
          <span class="page-info">Page {{ currentPage }} of {{ totalPages }}</span>
          <button 
            class="pagination-btn" 
            :disabled="currentPage === totalPages"
            @click="goToPage(currentPage + 1)"
          >
            <i class="fas fa-chevron-right"></i>
          </button>
        </div>
      </div>

      <!-- Removed Payment Details Modal -->

    </div>
  </div>
</template>

<script>
import AdminSidebar from '~/components/AdminSidebar.vue'
import { getPayment } from '~/server/payment-service'

export default {
  name: 'ManagePayment',
  components: { AdminSidebar },
  data() {
    return {
      searchQuery: '',
      statusFilter: '',
      payments: [],
      loading: true,
      error: null,
      currentPage: 1,
      itemsPerPage: 10,
    }
  },
  computed: {
    filteredPayments() {
      return this.payments.filter(payment => {
        const matchesSearch = 
          String(payment.orderId).toLowerCase().includes(this.searchQuery.toLowerCase());
        
        const matchesStatus = !this.statusFilter || 
          (payment.status !== undefined && payment.status !== null && this.formatStatus(payment.status).toLowerCase() === this.statusFilter.toLowerCase());
        
        return matchesSearch && matchesStatus
      });
    },
    paginatedPayments() {
      const start = (this.currentPage - 1) * this.itemsPerPage;
      const end = start + this.itemsPerPage;
      return this.filteredPayments.slice(start, end);
    },
    totalPages() {
      return Math.ceil(this.filteredPayments.length / this.itemsPerPage);
    },
    completedStats() {
      const completed = this.payments.filter(p => this.formatStatus(p.status) === 'COMPLETED');
      return {
        total: completed.reduce((sum, p) => sum + (p.amount || 0), 0),
        count: completed.length
      };
    },
    pendingStats() {
      const pending = this.payments.filter(p => this.formatStatus(p.status) === 'PENDING');
      return {
        total: pending.reduce((sum, p) => sum + (p.amount || 0), 0),
        count: pending.length
      };
    },
    failedStats() {
      const failed = this.payments.filter(p => this.formatStatus(p.status) === 'FAILED');
      return {
        total: failed.reduce((sum, p) => sum + (p.amount || 0), 0),
        count: failed.length
      };
    },
    refundedStats() {
      const refunded = this.payments.filter(p => this.formatStatus(p.status) === 'REFUNDED');
      return {
        total: refunded.reduce((sum, p) => sum + (p.amount || 0), 0),
        count: refunded.length
      };
    },
  },
  methods: {
    formatCurrency(amount) {
      return new Intl.NumberFormat('vi-VN', {
        style: 'currency',
        currency: 'VND',
        minimumFractionDigits: 0,
        maximumFractionDigits: 0
      }).format(amount);
    },
    // Removed formatDate and getInitials methods
    getPaymentMethodIcon(method) {
      // Map numeric payment method to icon class
      const icons = {
        0: 'fas fa-qrcode', // Example icon for PayOS (using qrcode icon)
        1: 'fas fa-wallet', // Example icon for Wallet
      }
      return icons[method] || 'fas fa-money-bill' // Default icon
    },
    formatPaymentMethod(method) {
      // Map numeric payment method to string
      const methodMap = {
        0: 'payOS',
        1: 'wallet',
      };
      return methodMap[method] || 'Unknown';
    },
    formatStatus(status) {
      // Map numeric status to string status
      const statusMap = {
        0: 'PENDING',
        1: 'COMPLETED',
        2: 'FAILED',
        // Add other mappings if needed based on API response
      };
      return statusMap[status] || 'Unknown';
    },
    // Removed handleSearch, handleFilter, viewPaymentDetails, closeModal, approvePayment, refundPayment
    goToPage(page) {
      if (page >= 1 && page <= this.totalPages) {
        this.currentPage = page;
      }
    },
  },
  async created() {
    try {
      this.loading = true;
      this.error = null;
      console.log('Fetching payments...');
      const response = await getPayment();
      console.log('Payment API Full Response:', response);
      
      // Check if response is an object and contains a 'payments' array directly
      if (response && typeof response === 'object' && Array.isArray(response.payments)) {
          // Assign the array of payment objects directly from response.payments
          this.payments = response.payments;
          console.log('Payments loaded successfully.', this.payments);
      } else {
          console.error('API response does not contain a valid payments array at response.payments:', response);
          this.error = 'Invalid data format from API';
      }
      
    } catch (error) {
      console.error('Error fetching payments:', error);
      this.error = 'Failed to load payments. Please try again.';
    } finally {
      this.loading = false;
      console.log('Payment fetching finished. Loading state:', this.loading);
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

/* Pagination Styles */
.pagination {
  display: flex;
  justify-content: center;
  align-items: center;
  padding: 20px;
  gap: 16px;
  border-top: 1px solid #e3e8ef;
}

.pagination-btn {
  width: 36px;
  height: 36px;
  border: 1px solid #e3e8ef;
  border-radius: 8px;
  background-color: white;
  color: #1a1f36;
  display: flex;
  align-items: center;
  justify-content: center;
  cursor: pointer;
  transition: all 0.2s ease;
}

.pagination-btn:hover:not(:disabled) {
  background-color: #f8fafc;
  border-color: #cbd5e1;
}

.pagination-btn:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}

.page-info {
  font-size: 14px;
  color: #697386;
}
</style> 