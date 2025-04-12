<template>
  <div class="admin-dashboard">
    <div class="dashboard-container">
      <AdminSidebar />
      <div class="main-content">
        <div class="dashboard-stats">
          <div class="stat-card">
            <div class="stat-icon">
              <ShoppingCartOutlined />
            </div>
            <div class="stat-info">
              <h3>Total Orders</h3>
              <p>1,234</p>
            </div>
          </div>
          <div class="stat-card">
            <div class="stat-icon">
              <AppstoreOutlined />
            </div>
            <div class="stat-info">
              <h3>Total Products</h3>
              <p>567</p>
            </div>
          </div>
          <div class="stat-card">
            <div class="stat-icon">
              <TeamOutlined />
            </div>
            <div class="stat-info">
              <h3>Total Users</h3>
              <p>890</p>
            </div>
          </div>
          <div class="stat-card">
            <div class="stat-icon">
              <DollarOutlined />
            </div>
            <div class="stat-info">
              <h3>Total Revenue</h3>
              <p>$45,678</p>
            </div>
          </div>
        </div>
        <div class="recent-orders">
          <h2>Recent Orders</h2>
          <a-table :columns="columns" :data-source="data" :pagination="{ pageSize: 5 }">
            <template #bodyCell="{ column, record }">
              <template v-if="column.key === 'status'">
                <a-tag :color="getStatusColor(record.status)">
                  {{ record.status }}
                </a-tag>
              </template>
            </template>
          </a-table>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import {
  ShoppingCartOutlined,
  AppstoreOutlined,
  UserOutlined,
  StarOutlined,
  CarOutlined,
  ExperimentOutlined,
  GiftOutlined,
  SettingOutlined,
  LogoutOutlined,
  DollarOutlined
} from '@ant-design/icons-vue';
import AdminSidebar from '~/components/AdminSidebar.vue';

const router = useRouter();

const columns = [
  {
    title: 'Order ID',
    dataIndex: 'id',
    key: 'id',
  },
  {
    title: 'Customer',
    dataIndex: 'customer',
    key: 'customer',
  },
  {
    title: 'Date',
    dataIndex: 'date',
    key: 'date',
  },
  {
    title: 'Amount',
    dataIndex: 'amount',
    key: 'amount',
  },
  {
    title: 'Status',
    dataIndex: 'status',
    key: 'status',
  },
];

const data = ref([
  {
    id: 'ORD-001',
    customer: 'John Doe',
    date: '2024-03-20',
    amount: '$120.00',
    status: 'Completed',
  },
  {
    id: 'ORD-002',
    customer: 'Jane Smith',
    date: '2024-03-19',
    amount: '$85.50',
    status: 'Processing',
  },
  {
    id: 'ORD-003',
    customer: 'Bob Johnson',
    date: '2024-03-18',
    amount: '$210.75',
    status: 'Pending',
  },
  {
    id: 'ORD-004',
    customer: 'Alice Brown',
    date: '2024-03-17',
    amount: '$65.25',
    status: 'Completed',
  },
  {
    id: 'ORD-005',
    customer: 'Charlie Wilson',
    date: '2024-03-16',
    amount: '$150.00',
    status: 'Processing',
  },
]);

const getStatusColor = (status) => {
  switch (status) {
    case 'Completed':
      return 'green';
    case 'Processing':
      return 'blue';
    case 'Pending':
      return 'orange';
    default:
      return 'default';
  }
};

const logout = () => {
  // Handle logout logic here
  router.push('/loginPage');
};
</script>

<style scoped>
.admin-dashboard {
  min-height: 100vh;
  background-color: #f0f2f5;
}

.dashboard-container {
  display: flex;
  min-height: 100vh;
  margin-top: 0;
}

.main-content {
  flex: 1;
  margin-left: 250px;
  padding: 24px;
}

.dashboard-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 24px;
}

.dashboard-header h1 {
  margin: 0;
  font-size: 24px;
  color: #333;
}

.user-info {
  display: flex;
  align-items: center;
  gap: 16px;
}

.dashboard-stats {
  display: grid;
  grid-template-columns: repeat(4, 1fr);
  gap: 24px;
  margin-bottom: 24px;
}

.stat-card {
  background-color: #fff;
  padding: 24px;
  border-radius: 8px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
  display: flex;
  align-items: center;
}

.stat-icon {
  font-size: 32px;
  color: #1890ff;
  margin-right: 16px;
}

.stat-info h3 {
  margin: 0;
  font-size: 14px;
  color: #666;
}

.stat-info p {
  margin: 8px 0 0;
  font-size: 24px;
  font-weight: bold;
  color: #333;
}

.recent-orders {
  background-color: #fff;
  padding: 24px;
  border-radius: 8px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
}

.recent-orders h2 {
  margin: 0 0 16px;
  font-size: 18px;
  color: #333;
}

@media (max-width: 1200px) {
  .dashboard-stats {
    grid-template-columns: repeat(2, 1fr);
  }
}

@media (max-width: 768px) {
  .dashboard-container {
    flex-direction: column;
  }

  .dashboard-stats {
    grid-template-columns: 1fr;
  }
}
</style>
