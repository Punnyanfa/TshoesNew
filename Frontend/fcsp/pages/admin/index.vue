<template>
  <div class="admin-dashboard">
    <div class="dashboard-container">
      <AdminSidebar />
      <div class="main-content">
        <div class="dashboard-header">
          <h1>Welcome Admin!</h1>
        </div>
        
        <!-- Statistics Cards -->
        <div class="stats-overview">
          <div class="stat-card">
            <div class="stat-icon">
              <ProjectOutlined />
            </div>
            <div class="stat-info">
              <p>{{ totalProducts }}</p>
              <h3>Products</h3>
            </div>
          </div>
          <div class="stat-card">
            <div class="stat-icon">
              <DollarOutlined />
            </div>
            <div class="stat-info">
              <p>{{ formatCurrency(totalRevenue) }}</p>
              <h3>Revenue</h3>
            </div>
          </div>
          <div class="stat-card">
            <div class="stat-icon">
              <CheckCircleOutlined />
            </div>
            <div class="stat-info">
              <p>{{ totalOrders }}</p>
              <h3>Orders</h3>
            </div>
          </div>
          <div class="stat-card">
            <div class="stat-icon">
              <TeamOutlined />
            </div>
            <div class="stat-info">
              <p>{{ totalUsers }}</p>
              <h3>Users</h3>
            </div>
          </div>
        </div>

        <!-- Charts Section -->
        <div class="charts-section">
          <div class="chart-container">
            <h2>Total Revenue</h2>
            <div id="revenue-chart"></div>
          </div>
          <div class="chart-container">
            <h2>Sales Overview</h2>
            <div id="sales-chart"></div>
          </div>
        </div>

        <!-- Performance Metrics -->
        <div class="performance-metrics">
          <div class="metric-card">
            <div class="metric-header">
              <h3>New Users</h3>
              <span class="percentage positive">+{{ metrics.newUsers.percentage }}%</span>
            </div>
            <div class="metric-value">{{ metrics.newUsers.value }}</div>
          </div>
          <div class="metric-card">
            <div class="metric-header">
              <h3>Earnings</h3>
              <span class="percentage positive">+{{ metrics.earnings.percentage }}%</span>
            </div>
            <div class="metric-value">{{ formatCurrency(metrics.earnings.value) }}</div>
          </div>
          <div class="metric-card">
            <div class="metric-header">
              <h3>Expenses</h3>
              <span class="percentage negative">-{{ metrics.expenses.percentage }}%</span>
            </div>
            <div class="metric-value">{{ formatCurrency(metrics.expenses.value) }}</div>
          </div>
          <div class="metric-card">
            <div class="metric-header">
              <h3>Profit</h3>
              <span class="percentage positive">+{{ metrics.profit.percentage }}%</span>
            </div>
            <div class="metric-value">{{ formatCurrency(metrics.profit.value) }}</div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue';
import { useRouter } from 'vue-router';
import { Line, Column } from '@antv/g2plot';
import {
  ShoppingCartOutlined,
  AppstoreOutlined,
  TeamOutlined,
  DollarOutlined,
  ProjectOutlined,
  CheckCircleOutlined,
} from '@ant-design/icons-vue';
import AdminSidebar from '~/components/AdminSidebar.vue';
import { getAllProducts } from '~/api/product';
import { getAllOrders } from '~/api/order';
import { getAllAccounts } from '~/api/account';
import { getAllComments } from '~/api/comment';

const router = useRouter();

// Statistics Data
const totalProducts = ref(0);
const totalRevenue = ref(0);
const totalOrders = ref(0);
const totalUsers = ref(0);
const averageRating = ref(0);
const pendingOrders = ref(0);

// Revenue Chart Data
const revenueData = ref([]);
const salesData = ref([]);

// Performance Metrics
const metrics = ref({
  newUsers: { value: 0, percentage: 0 },
  earnings: { value: 0, percentage: 0 },
  expenses: { value: 0, percentage: 0 },
  profit: { value: 0, percentage: 0 }
});

const formatCurrency = (value) => {
  return new Intl.NumberFormat('vi-VN', {
    style: 'currency',
    currency: 'VND'
  }).format(value);
};

const calculateMonthlyData = (orders) => {
  const monthlyData = {};
  orders.forEach(order => {
    const date = new Date(order.createdAt);
    const monthYear = `${date.getFullYear()}-${date.getMonth() + 1}`;
    
    if (!monthlyData[monthYear]) {
      monthlyData[monthYear] = {
        revenue: 0,
        orders: 0
      };
    }
    
    monthlyData[monthYear].revenue += order.totalPrice;
    monthlyData[monthYear].orders += 1;
  });
  
  return monthlyData;
};

onMounted(async () => {
  try {
    // Fetch all necessary data
    const [products, orders, accounts, comments] = await Promise.all([
      getAllProducts(),
      getAllOrders(),
      getAllAccounts(),
      getAllComments()
    ]);

    // Update statistics
    if (products?.data?.designs) {
      totalProducts.value = products.data.designs.length;
    }

    if (Array.isArray(orders)) {
      totalOrders.value = orders.length;
      totalRevenue.value = orders.reduce((sum, order) => sum + order.totalPrice, 0);
      pendingOrders.value = orders.filter(order => order.statusName === 'Pending').length;
      
      // Calculate monthly data for charts
      const monthlyData = calculateMonthlyData(orders);
      
      // Prepare data for revenue chart
      revenueData.value = Object.entries(monthlyData).map(([month, data]) => ({
        month,
        value: data.revenue,
        type: 'Revenue'
      }));
      
      // Prepare data for sales chart
      salesData.value = Object.entries(monthlyData).map(([month, data]) => ({
        month,
        value: data.orders,
        category: 'Orders'
      }));
    }

    if (Array.isArray(accounts)) {
      totalUsers.value = accounts.length;
      metrics.value.newUsers.value = accounts.filter(acc => {
        const createdDate = new Date(acc.createdAt);
        const thirtyDaysAgo = new Date();
        thirtyDaysAgo.setDate(thirtyDaysAgo.getDate() - 30);
        return createdDate >= thirtyDaysAgo;
      }).length;
    }

    if (Array.isArray(comments)) {
      const totalRating = comments.reduce((sum, comment) => sum + comment.userRating, 0);
      averageRating.value = comments.length > 0 ? (totalRating / comments.length).toFixed(1) : 0;
    }

    // Calculate performance metrics
    const lastMonthRevenue = orders
      .filter(order => {
        const orderDate = new Date(order.createdAt);
        const lastMonth = new Date();
        lastMonth.setMonth(lastMonth.getMonth() - 1);
        return orderDate >= lastMonth;
      })
      .reduce((sum, order) => sum + order.totalPrice, 0);

    metrics.value.earnings.value = lastMonthRevenue;
    metrics.value.earnings.percentage = 5.5;
    metrics.value.expenses.value = lastMonthRevenue * 0.3; // Assuming 30% expenses
    metrics.value.expenses.percentage = -2.4;
    metrics.value.profit.value = lastMonthRevenue * 0.7; // Assuming 70% profit
    metrics.value.profit.percentage = 9;

    // Initialize Revenue Chart
    const revenueChart = new Column('revenue-chart', {
      data: revenueData.value,
      xField: 'month',
      yField: 'value',
      seriesField: 'type',
      isStack: true,
      label: {
        position: 'middle',
      },
      legend: {
        position: 'top',
      },
    });
    revenueChart.render();

    // Initialize Sales Chart
    const salesChart = new Line('sales-chart', {
      data: salesData.value,
      xField: 'month',
      yField: 'value',
      seriesField: 'category',
      smooth: true,
      legend: {
        position: 'top',
      },
      point: {
        size: 5,
        shape: 'diamond',
      },
    });
    salesChart.render();
  } catch (error) {
    console.error('Error fetching dashboard data:', error);
  }
});
</script>

<style scoped>
.admin-dashboard {
  min-height: 100vh;
  background-color: #f0f2f5;
}

.dashboard-container {
  display: flex;
  min-height: 100vh;
}

.main-content {
  flex: 1;
  margin-left: 250px;
  padding: 24px;
}

.dashboard-header {
  margin-bottom: 24px;
}

.dashboard-header h1 {
  font-size: 24px;
  color: #1a1a1a;
}

.stats-overview {
  display: grid;
  grid-template-columns: repeat(4, 1fr);
  gap: 20px;
  margin-bottom: 24px;
}

.stat-card {
  background: white;
  padding: 20px;
  border-radius: 10px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
  display: flex;
  align-items: center;
}

.stat-icon {
  font-size: 24px;
  color: #1890ff;
  margin-right: 16px;
}

.stat-info p {
  font-size: 24px;
  font-weight: bold;
  margin: 0;
  color: #1a1a1a;
}

.stat-info h3 {
  font-size: 14px;
  color: #8c8c8c;
  margin: 0;
}

.charts-section {
  display: grid;
  grid-template-columns: repeat(2, 1fr);
  gap: 20px;
  margin-bottom: 24px;
}

.chart-container {
  background: white;
  padding: 20px;
  border-radius: 10px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
}

.chart-container h2 {
  margin-bottom: 20px;
  font-size: 18px;
  color: #1a1a1a;
}

.performance-metrics {
  display: grid;
  grid-template-columns: repeat(4, 1fr);
  gap: 20px;
}

.metric-card {
  background: white;
  padding: 20px;
  border-radius: 10px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
}

.metric-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 10px;
}

.metric-header h3 {
  font-size: 14px;
  color: #8c8c8c;
  margin: 0;
}

.percentage {
  font-size: 14px;
  font-weight: bold;
}

.percentage.positive {
  color: #52c41a;
}

.percentage.negative {
  color: #f5222d;
}

.metric-value {
  font-size: 24px;
  font-weight: bold;
  color: #1a1a1a;
}

@media (max-width: 1200px) {
  .stats-overview,
  .charts-section,
  .performance-metrics {
    grid-template-columns: repeat(2, 1fr);
  }
}

@media (max-width: 768px) {
  .stats-overview,
  .charts-section,
  .performance-metrics {
    grid-template-columns: 1fr;
  }
  
  .main-content {
    margin-left: 0;
  }
}
</style>