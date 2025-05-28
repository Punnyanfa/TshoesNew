<template>
  <div class="reset-password-container">
    <div class="reset-password-box">
      <h2 class="title">Reset Your Password</h2>
      <form @submit.prevent="submitReset">
        <div class="form-group">
          <label>New Password</label>
          <input type="password" v-model="newPassword" class="form-control" placeholder="Enter new password" required />
        </div>
        <div class="form-group">
          <label>Confirm New Password</label>
          <input type="password" v-model="confirmPassword" class="form-control" placeholder="Confirm new password" required />
        </div>
        <div v-if="errorMessage" class="error-message">{{ errorMessage }}</div>
        <button type="submit" class="btn btn-primary w-100" :disabled="isLoading">
          <span v-if="isLoading" class="spinner-border spinner-border-sm me-2"></span>
          Change Password
        </button>
      </form>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue';
import { useRouter, useRoute } from 'vue-router';
// import { resetPassword } from "@/server/auth/forgotPassword-service"; // Bạn cần tạo hàm này để gọi API

const router = useRouter();
const route = useRoute();
const newPassword = ref('');
const confirmPassword = ref('');
const errorMessage = ref('');
const isLoading = ref(false);
// Nhận email hoặc token từ query
const email = route.query.email || '';
// const token = route.query.token || '';

const submitReset = async () => {
  errorMessage.value = '';
  if (newPassword.value.length < 8) {
    errorMessage.value = 'Password must be at least 8 characters!';
    return;
  }
  if (newPassword.value !== confirmPassword.value) {
    errorMessage.value = 'Passwords do not match!';
    return;
  }
  isLoading.value = true;
  try {
    // await resetPassword({ email, newPassword: newPassword.value });
    // Nếu backend yêu cầu token, truyền thêm token
    // await resetPassword({ token, newPassword: newPassword.value });
    // Giả lập thành công:
    setTimeout(() => {
      isLoading.value = false;
      router.push('/loginPage');
    }, 1200);
  } catch (e) {
    errorMessage.value = e.response?.data?.message || 'Failed to reset password!';
    isLoading.value = false;
  }
};
</script>

<style scoped>
.reset-password-container {
  min-height: 100vh;
  display: flex;
  align-items: center;
  justify-content: center;
  background: linear-gradient(135deg, #f5f7fa 0%, #e4e9f2 100%);
}
.reset-password-box {
  background: #fff;
  border-radius: 16px;
  box-shadow: 0 4px 24px rgba(30,41,59,0.13);
  padding: 2.5rem 2rem 2rem 2rem;
  max-width: 350px;
  width: 100%;
}
.title {
  text-align: center;
  font-size: 1.5rem;
  font-weight: 700;
  margin-bottom: 1.5rem;
  color: #2c3e50;
}
.form-group {
  margin-bottom: 1.2rem;
}
.form-control {
  width: 100%;
  padding: 0.7rem 1rem;
  border-radius: 8px;
  border: 1.5px solid #e3e8ee;
  font-size: 1rem;
  transition: border-color 0.2s, box-shadow 0.2s;
}
.form-control:focus {
  border-color: #AAAAAA;
  box-shadow: 0 0 0 2px rgba(170, 170, 170, 0.10);
}
.btn-primary {
  background: linear-gradient(90deg, #AAAAAA 0%, #2c3e50 100%);
  border: none;
  border-radius: 50px;
  font-weight: 600;
  padding: 0.6rem 2.2rem;
  letter-spacing: 0.5px;
  box-shadow: 0 2px 8px rgba(170, 170, 170, 0.10);
  transition: background 0.2s, box-shadow 0.2s, transform 0.2s;
}
.btn-primary:hover {
  background: linear-gradient(90deg, #2c3e50 0%, #AAAAAA 100%);
  box-shadow: 0 4px 16px rgba(170, 170, 170, 0.18);
  transform: translateY(-2px);
}
.error-message {
  color: #EF4444;
  font-size: 0.97rem;
  margin-bottom: 0.7rem;
  text-align: center;
}
</style> 