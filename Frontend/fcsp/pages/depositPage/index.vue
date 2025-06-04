<template>
  <Header />
  <div class="deposit-container">
    <div class="deposit-card">
      <h1 class="deposit-title">Deposit Money</h1>
      <form @submit.prevent="handleDeposit">
        <div class="form-group">
          <label for="amount">Amount (VND)</label>
          <input v-model.number="form.amount" id="amount" type="number" min="1000" step="1000" required placeholder="Enter amount (VND)" />
        </div>
        <button class="deposit-btn" :disabled="loading">
          <span v-if="loading"><i class="fas fa-spinner fa-spin"></i> Processing...</span>
          <span v-else>Deposit</span>
        </button>
      </form>
      <div v-if="message" :class="['message', messageType]">
        {{ message }}
      </div>
    </div>
  </div>
</template>

<script>
import { rechargePayment } from '~/server/payment-service'

export default {
  name: 'DepositPage',
  data() {
    return {
      form: {
        paymentId: 0,
        amount: ''
      },
      loading: false,
      message: '',
      messageType: ''
    }
  },
  methods: {
    async handleDeposit() {
      this.loading = true;
      this.message = '';
      this.messageType = '';
      try {
        const res = await rechargePayment({
          userId: this.form.userId,
          paymentId: this.form.paymentId,
          amount: this.form.amount
        });
        // Nếu có link thanh toán trả về từ API thì redirect
        if (res && res.response) {
          window.location.href = res.response;
          return;
        }
        this.message = 'Deposit successful!';
        this.messageType = 'success';
        this.form.amount = '';
      } catch (error) {
        this.message = error.message || 'Deposit failed!';
        this.messageType = 'error';
      } finally {
        this.loading = false;
      }
    }
  },
  mounted() {
    // Lấy userId từ localStorage (giả sử lưu dưới key 'userId')
    const userId = localStorage.getItem('userId');
    if (userId) {
      this.form.userId = Number(userId);
    } else {
      this.message = 'User ID not found in localStorage!';
      this.messageType = 'error';
    }
  }
}
</script>

<style scoped>
.deposit-container {
  min-height: 100vh;
  display: flex;
  align-items: center;
  justify-content: center;
  background: linear-gradient(135deg, #f0f4ff 0%, #e0e7ff 100%);
}
.deposit-card {
  background: #fff;
  padding: 40px 32px;
  border-radius: 16px;
  box-shadow: 0 4px 24px rgba(79, 70, 229, 0.08);
  width: 100%;
  max-width: 400px;
  display: flex;
  flex-direction: column;
  align-items: center;
}
.deposit-title {
  font-size: 2rem;
  font-weight: 700;
  color: #4f46e5;
  margin-bottom: 32px;
}
.form-group {
  width: 100%;
  margin-bottom: 20px;
  display: flex;
  flex-direction: column;
}
.form-group label {
  font-size: 1rem;
  color: #697386;
  margin-bottom: 8px;
  font-weight: 500;
}
.form-group input,
.form-group select {
  padding: 12px 14px;
  border: 1px solid #e3e8ef;
  border-radius: 8px;
  font-size: 1rem;
  background: #f8fafc;
  transition: border 0.2s;
}
.form-group input:focus,
.form-group select:focus {
  border-color: #4f46e5;
  outline: none;
  background: #fff;
}
.deposit-btn {
  width: 100%;
  padding: 14px;
  background: #4f46e5;
  color: #fff;
  font-size: 1.1rem;
  font-weight: 600;
  border: none;
  border-radius: 8px;
  cursor: pointer;
  transition: background 0.2s;
  margin-top: 10px;
  display: flex;
  align-items: center;
  justify-content: center;
}
.deposit-btn:disabled {
  background: #a5b4fc;
  cursor: not-allowed;
}
.message {
  margin-top: 24px;
  padding: 12px 18px;
  border-radius: 8px;
  font-size: 1rem;
  width: 100%;
  text-align: center;
}
.message.success {
  background: #dcfce7;
  color: #16a34a;
  border: 1px solid #bbf7d0;
}
.message.error {
  background: #fee2e2;
  color: #dc2626;
  border: 1px solid #fecaca;
}
</style>

