<template>
  <div class="register-container">
    <div class="register-box">
      <!-- Phần trái: Hình ảnh sneaker -->
      <div class="register-image">
        <img src="https://th.bing.com/th/id/OIP.hmwhi4IHiABkRdfZSbxQ2AHaHa?w=160&h=180&c=7&r=0&o=5&dpr=1.3&pid=1.7" alt="Sneaker Image" />
      </div>

      <!-- Phần phải: Form đăng ký -->
      <div class="register-form">
        <h1 class="brand-name">Create Your Account</h1>
        <p class="subtitle">Join us to design your custom sneakers</p>

        <form @submit.prevent="register">
          <div class="mb-3">
            <label class="form-label">Name</label>
            <div class="input-group">
              <UserOutlined class="input-icon" />
              <input v-model="name" type="text" class="form-control" :class="{ 'is-invalid': nameError }" placeholder="Enter your name" required />
            </div>
            <div v-if="nameError" class="error-message">{{ nameError }}</div>
          </div>

          <div class="mb-3">
            <label class="form-label">Email</label>
            <div class="input-group">
              <MailOutlined class="input-icon" />
              <input v-model="email" type="email" class="form-control" :class="{ 'is-invalid': emailError }" placeholder="Enter your email" required />
            </div>
            <div v-if="emailError" class="error-message">{{ emailError }}</div>
          </div>

          <div class="mb-3">
            <label class="form-label">Password</label>
            <div class="input-group">
              <LockOutlined class="input-icon" />
              <input v-model="password" :type="showPassword ? 'text' : 'password'" class="form-control" :class="{ 'is-invalid': passwordError }" placeholder="Create a password" required />
              <button type="button" class="toggle-password" @click="togglePassword">
                <EyeOutlined v-if="!showPassword" />
                <EyeInvisibleOutlined v-else />
              </button>
            </div>
            <div v-if="passwordError" class="error-message">{{ passwordError }}</div>
          </div>

          <button type="submit" class="btn btn-custom w-100">Sign Up</button>
        </form>

        <p class="text-center mt-3">
          Already have an account? <router-link to="/loginPage" class="link">Login</router-link>
        </p>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue';
import {
  UserOutlined,
  MailOutlined,
  LockOutlined,
  EyeOutlined,
  EyeInvisibleOutlined
} from '@ant-design/icons-vue';

const name = ref('');
const email = ref('');
const password = ref('');
const showPassword = ref(false);
const nameError = ref('');
const emailError = ref('');
const passwordError = ref('');

const validateInputs = () => {
  nameError.value = name.value ? '' : 'Name is required';
  emailError.value = email.value ? '' : 'Email is required';
  passwordError.value = password.value ? '' : 'Password is required';
  return !nameError.value && !emailError.value && !passwordError.value;
};

const togglePassword = () => {
  showPassword.value = !showPassword.value;
};

const register = () => {
  if (!validateInputs()) return;
  console.log(`Registering ${name.value}`);
};
</script>

<style scoped>
.register-container {
  display: flex;
  justify-content: center;
  align-items: center;
  height: 100vh;
  background: #f5f5f5;
}

.register-box {
  display: flex;
  width: 900px;
  height: 600px;
  background: white;
  border-radius: 15px;
  box-shadow: 0 4px 15px rgba(0, 0, 0, 0.3);
  overflow: hidden;
}

/* 🏀 Phần trái: Hình ảnh sneaker */
.register-image {
  width: 50%;
  background: linear-gradient(135deg, #ff5722, #ff8a50);
  display: flex;
  justify-content: center;
  align-items: center;
}

.register-image img {
  width: 90%;
  border-radius: 10px;
  box-shadow: 0 8px 20px rgba(0, 0, 0, 0.3);
}

/* 🎨 Phần phải: Form đăng ký */
.register-form {
  width: 50%;
  padding: 2rem;
  display: flex;
  flex-direction: column;
  justify-content: center;
}

.brand-name {
  font-size: 2rem;
  font-weight: bold;
  color: #ff5722;
  text-transform: uppercase;
  text-align: center;
}

.subtitle {
  font-size: 1rem;
  color: #555;
  margin-bottom: 20px;
  text-align: center;
}

.input-group {
  display: flex;
  align-items: center;
  position: relative;
}

.input-icon {
  position: absolute;
  left: 10px;
  color: #ff5722;
  font-size: 20px;
  z-index: 1;
}

input {
  width: 100%;
  padding-left: 40px;
  border-radius: 5px;
  transition: 0.3s ease-in-out;
}

input:focus {
  border-color: #ff5722;
  box-shadow: 0 0 8px rgba(255, 87, 34, 0.6);
}

.toggle-password {
  position: absolute;
  right: 10px;
  background: none;
  border: none;
  cursor: pointer;
  font-size: 20px;
}

.error-message {
  color: red;
  font-size: 0.875rem;
  text-align: left;
  margin-top: 5px;
}

.btn-custom {
  background: #ff5722;
  color: white;
  font-weight: bold;
  text-transform: uppercase;
  transition: 0.3s;
  border-radius: 30px;
  padding: 12px;
}

.btn-custom:hover {
  background: #e64a19;
  transform: scale(1.05);
}

.link {
  color: #ff5722;
  text-decoration: none;
  font-weight: bold;
}

.link:hover {
  text-decoration: underline;
}
</style>
