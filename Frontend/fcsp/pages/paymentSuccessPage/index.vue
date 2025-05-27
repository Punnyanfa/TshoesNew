<template>
  <div class="success-page">
    <h1>Success</h1>
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
      // alert('Đã cập nhật trạng thái thanh toán thành công!');
    })
    .catch(() => {
      // alert('Cập nhật trạng thái thất bại!');
    });
}
</script>


