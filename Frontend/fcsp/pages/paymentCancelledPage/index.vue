<template>
  <div class="cancelled-page">
    <div class="cancelled-container">
      <div class="cancelled-icon">
        <i class="bi bi-x-circle-fill"></i>
      </div>
      <h1 class="cancelled-title">Payment Cancelled</h1>
      <p class="cancelled-message">Your payment has been cancelled. If you have any questions, please contact our support team.</p>
      <!-- <div class="order-details">
        <p>Order Code: <span class="order-code">{{ orderCode }}</span></p>
        <p>Status: <span class="order-status">{{ status }}</span></p>
      </div> -->
      <NuxtLink to="/" class="home-button">
        <i class="bi bi-house-door"></i>
        Back to Home
      </NuxtLink>
    </div>
  </div>
</template>

<script setup>
import { useRouter } from 'vue-router';
import { putPaymentWebhook } from '@/server/order-service';

const router = useRouter();
const query = router.currentRoute.value.query;
const status = query.status;
const orderCode = query.orderCode;
console.log('orderCode:', orderCode, 'status:', status);
if (status && orderCode) {
  putPaymentWebhook(orderCode, status)
    .then(() => {
      // alert('Đã cập nhật trạng thái hủy thanh toán!');
    })
    .catch(() => {
      // alert('Cập nhật trạng thái thất bại!');
    });
}
</script>

<style scoped>
.cancelled-page {
  min-height: 100vh;
  display: flex;
  align-items: center;
  justify-content: center;
  background: linear-gradient(135deg, #f5f7fa 0%, #c3cfe2 100%);
  padding: 20px;
}

.cancelled-container {
  background: white;
  padding: 40px;
  border-radius: 20px;
  box-shadow: 0 10px 30px rgba(0, 0, 0, 0.1);
  text-align: center;
  max-width: 500px;
  width: 100%;
  animation: slideUp 0.5s ease;
}

.cancelled-icon {
  font-size: 80px;
  color: #dc3545;
  margin-bottom: 20px;
  animation: scaleIn 0.5s ease;
}

.cancelled-title {
  color: #2c3e50;
  font-size: 28px;
  font-weight: bold;
  margin-bottom: 15px;
}

.cancelled-message {
  color: #666;
  font-size: 16px;
  line-height: 1.6;
  margin-bottom: 30px;
}

.order-details {
  background: #f8f9fa;
  padding: 20px;
  border-radius: 10px;
  margin-bottom: 30px;
}

.order-details p {
  margin: 10px 0;
  color: #555;
}

.order-code, .order-status {
  font-weight: bold;
  color: #2c3e50;
}

.home-button {
  display: inline-flex;
  align-items: center;
  gap: 8px;
  background: linear-gradient(45deg, #AAAAAA, #888888);
  color: white;
  padding: 12px 30px;
  border-radius: 25px;
  text-decoration: none;
  font-weight: 600;
  transition: all 0.3s ease;
}

.home-button:hover {
  transform: translateY(-2px);
  box-shadow: 0 5px 15px rgba(0, 0, 0, 0.2);
  color: white;
}

.home-button i {
  font-size: 18px;
}

@keyframes slideUp {
  from {
    opacity: 0;
    transform: translateY(20px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

@keyframes scaleIn {
  from {
    transform: scale(0);
  }
  to {
    transform: scale(1);
  }
}

@media (max-width: 480px) {
  .cancelled-container {
    padding: 30px 20px;
  }
  
  .cancelled-icon {
    font-size: 60px;
  }
  
  .cancelled-title {
    font-size: 24px;
  }
  
  .cancelled-message {
    font-size: 14px;
  }
}
</style>

