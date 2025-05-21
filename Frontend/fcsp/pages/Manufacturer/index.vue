<template>
  <div>
    <HeaderManu @logout="logout" />
    <div class="manufacturer-layout">
      <div class="main-content">
        <!-- Welcome Banner -->
        <div class="welcome-banner">
          <h1>Welcome to Manufacturer Dashboard</h1>
          <p>Manage your orders and services efficiently</p>
        </div>

        <!-- Quick Stats -->
        <div class="row mt-4">
          <div class="col-md-4">
            <div class="stat-box">
              <div class="stat-icon">
                <i class="bi bi-cart3"></i>
              </div>
              <div class="stat-content">
                <h3>{{ totalOrders }}</h3>
                <p>Total Orders</p>
              </div>
            </div>
          </div>
          <div class="col-md-4">
            <div class="stat-box">
              <div class="stat-icon">
                <i class="bi bi-currency-dollar"></i>
              </div>
              <div class="stat-content">
                <h3>{{ formatCurrency(totalRevenue) }}</h3>
                <p>Total Revenue</p>
              </div>
            </div>
          </div>
          <div class="col-md-4">
            <div class="stat-box">
              <div class="stat-icon">
                <i class="bi bi-clock-history"></i>
              </div>
              <div class="stat-content">
                <h3>{{ pendingOrders }}</h3>
                <p>Pending Orders</p>
              </div>
            </div>
          </div>
        </div>

        <!-- Quick Links -->
        <div class="row mt-4">
          <div class="col-md-6">
            <div class="quick-link-card">
              <h4>Order Management</h4>
              <p>View and manage all your orders</p>
              <router-link to="/manufacturer/manageOrder" class="btn btn-primary">
                Manage Orders
              </router-link>
            </div>
          </div>
          <div class="col-md-6">
            <div class="quick-link-card">
              <h4>Service Management</h4>
              <p>Manage your services and fees</p>
              <router-link to="/manufacturer/manageService" class="btn btn-primary">
                Manage Services
              </router-link>
            </div>
          </div>
        </div>

        <!-- Recent Orders -->
        <div class="recent-orders mt-4">
          <h4>Recent Orders</h4>
          <div class="table-responsive">
            <table class="table">
              <thead>
                <tr>
                  <th>Order ID</th>
                  <th>Customer</th>
                  <th>Amount</th>
                  <th>Status</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="order in recentOrders" :key="order.id">
                  <td>#{{ order.id }}</td>
                  <td>{{ order.customer }}</td>
                  <td>{{ formatCurrency(order.amount) }}</td>
                  <td>
                    <span :class="['badge', getStatusClass(order.status)]">
                      {{ order.status }}
                    </span>
                  </td>
                </tr>
              </tbody>
            </table>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import HeaderManu from '@/components/HeaderManu.vue';

export default {
  name: 'ManufacturerHome',
  components: { HeaderManu },
  data() {
    return {
      totalOrders: 1234,
      totalRevenue: 45678900,
      pendingOrders: 23,
      recentOrders: [
        { id: 1001, customer: 'John Doe', amount: 1500000, status: 'Processing' },
        { id: 1002, customer: 'Jane Smith', amount: 2300000, status: 'Delivered' },
        { id: 1003, customer: 'Mike Johnson', amount: 950000, status: 'Pending' },
        { id: 1004, customer: 'Sarah Wilson', amount: 3200000, status: 'Shipping' },
        { id: 1005, customer: 'David Brown', amount: 1800000, status: 'Processing' }
      ]
    }
  },
  methods: {
    formatCurrency(amount) {
      return new Intl.NumberFormat('vi-VN', {
        style: 'currency',
        currency: 'VND'
      }).format(amount);
    },
    getStatusClass(status) {
      const classes = {
        'Pending': 'bg-warning',
        'Processing': 'bg-info',
        'Shipping': 'bg-primary',
        'Delivered': 'bg-success',
        'Cancelled': 'bg-danger'
      };
      return classes[status] || 'bg-secondary';
    },
    logout() {
      this.$router.push('/login');
    }
  }
}
</script>

<style>
.manufacturer-layout {
  min-height: 100vh;
  background-color: #f8f9fa;
}

.main-content {
  padding: 2rem;
}

.welcome-banner {
  background-color: #0d6efd;
  color: white;
  padding: 2rem;
  border-radius: 0.5rem;
  text-align: center;
}

.welcome-banner h1 {
  font-size: 2rem;
  margin-bottom: 0.5rem;
}

.welcome-banner p {
  font-size: 1.1rem;
  opacity: 0.9;
  margin: 0;
}

.stat-box {
  background: white;
  padding: 1.5rem;
  border-radius: 0.5rem;
  box-shadow: 0 2px 4px rgba(0,0,0,0.1);
  display: flex;
  align-items: center;
}

.stat-icon {
  font-size: 2rem;
  color: #0d6efd;
  margin-right: 1rem;
}

.stat-content h3 {
  font-size: 1.5rem;
  margin: 0;
  font-weight: 600;
}

.stat-content p {
  margin: 0;
  color: #6c757d;
}

.quick-link-card {
  background: white;
  padding: 1.5rem;
  border-radius: 0.5rem;
  box-shadow: 0 2px 4px rgba(0,0,0,0.1);
  height: 100%;
}

.quick-link-card h4 {
  margin-bottom: 0.5rem;
}

.quick-link-card p {
  color: #6c757d;
  margin-bottom: 1rem;
}

.recent-orders {
  background: white;
  padding: 1.5rem;
  border-radius: 0.5rem;
  box-shadow: 0 2px 4px rgba(0,0,0,0.1);
}

.recent-orders h4 {
  margin-bottom: 1rem;
}

.table th {
  font-weight: 600;
  background-color: #f8f9fa;
}

.badge {
  padding: 0.5em 0.75em;
  font-weight: 500;
}

@media (max-width: 768px) {
  .main-content {
    padding: 1rem;
  }
  
  .welcome-banner {
    padding: 1.5rem;
  }
  
  .welcome-banner h1 {
    font-size: 1.5rem;
  }
  
  .stat-box {
    margin-bottom: 1rem;
  }
}
</style>
