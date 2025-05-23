<template>
  <div class="login-container">
    <div class="login-box">
      <!-- Phần trái: Form đăng nhập -->
      <div class="login-form">
        <h1 class="brand-name">Custom Shoe Login</h1>
        <p class="subtitle">Design Your Dream Sneakers</p>

        <form @submit.prevent="login">
          <div class="mb-3">
            <label class="form-label">Email</label>
            <div class="input-group">
              <MailOutlined class="input-icon" />
              <input
                v-model="email"
                type="email"
                class="form-control"
                :class="{ 'is-invalid': emailError }"
                placeholder="Enter your email"
                required
              />
            </div>
            <div v-if="emailError" class="error-message">{{ emailError }}</div>
          </div>

          <div class="mb-3">
            <label class="form-label">Password</label>
            <div class="input-group">
              <LockOutlined class="input-icon" />
              <input
                v-model="password"
                :type="showPassword ? 'text' : 'password'"
                class="form-control"
                :class="{ 'is-invalid': passwordError }"
                placeholder="Enter your password"
                required
              />
              <button 
                type="button" 
                class="toggle-password" 
                @click="togglePassword"
              >
                <component :is="showPassword ? EyeInvisibleOutlined : EyeOutlined" />
              </button>
            </div>
            <div v-if="passwordError" class="error-message">{{ passwordError }}</div>
          </div>

          <button type="submit" class="btn btn-custom w-100">Login</button>
        </form>

        <p class="text-center mt-3">
          Don't have an account?
          <router-link to="/registerPage" class="link">Sign Up</router-link>
        </p>
      </div>

      <!-- Phần phải: Hình ảnh sneaker -->
      <div class="login-image">
        <img src="https://th.bing.com/th/id/OIP.hmwhi4IHiABkRdfZSbxQ2AHaHa?w=160&h=180&c=7&r=0&o=5&dpr=1.3&pid=1.7" alt="Sneaker Image" />
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import { loginUser } from '@/server/auth/login-service';
import { ElNotification } from 'element-plus'

// Import Ant Design Icons
import {
  MailOutlined,
  LockOutlined,
  EyeOutlined,
  EyeInvisibleOutlined
} from '@ant-design/icons-vue';

// State management
const email = ref('');
const password = ref('');
const showPassword = ref(false);
const emailError = ref('');
const passwordError = ref('');
const router = useRouter();

// Validation and UI handlers
const validateInputs = () => {
  emailError.value = email.value ? '' : 'Email is required';
  passwordError.value = password.value ? '' : 'Password is required';
  return !emailError.value && !passwordError.value;
};

const togglePassword = () => {
  showPassword.value = !showPassword.value;
};

// Login handler
const login = async () => {
  if (!validateInputs()) return;
  
  try {
    const result = await loginUser(email.value, password.value);
    
    // Get the role from localStorage
    const role = localStorage.getItem("role");
    
    // Define route mapping based on role
    let redirectRoute = '/homePage'; // Default route
    
    if (role === "Admin") {
      redirectRoute = '/adminDashboard';
    } else if (role === "Manufacturer") {
      redirectRoute = '/ManufacturerPage';
    }
    
    if (result === "Login successful" || result === "Admin login") {
      ElNotification({
        title: 'Success',
        message: 'Login successful! Redirecting...',
        type: 'success',
      });
      
      // Add a small delay before redirecting
      setTimeout(() => {
        router.push(redirectRoute);
      }, 500);
    } else {
      ElNotification({
        title: 'Error',
        message: 'Login failed. Please check your credentials.',
        type: 'error',
      });
    }
  } catch (error) {
    console.error('Login error:', error);
    ElNotification({
      title: 'Error',
      message: 'An error occurred during login. Please try again.',
      type: 'error',
    });
  }
};
</script>

<style scoped>
/* 🌟 Background */
.login-container {
  display: flex;
  justify-content: center;
  align-items: center;
  height: 100vh;
  background: #f5f5f5;
  border-radius: none;
}

/* 📦 Login Box (Chia 2 cột) */
.login-box {
  display: flex;
  width: 100%;
  height: 100vh;
  background: white;
  border-radius: 0;
  box-shadow: none;
  overflow: hidden;
}

/* 🎨 Phần trái: Form đăng nhập */
.login-form {
  width: 50%;
  padding: 4rem;
  display: flex;
  flex-direction: column;
  justify-content: center;
}

/* 🔥 Branding */
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

/* 🔑 Input Group */
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
  border: none;
  border-radius: 0;
  transition: 0.3s ease-in-out;
  background-color: #f5f5f5;
}

input:focus {
  border: none;
  box-shadow: none;
  background-color: #ebebeb;
  outline: none;
}

/* 👁️ Toggle Password */
.toggle-password {
  position: absolute;
  right: 10px;
  background: none;
  border: none;
  cursor: pointer;
  font-size: 20px;
}

/* ❌ Error Message */
.error-message {
  color: red;
  font-size: 0.875rem;
  text-align: left;
  margin-top: 5px;
}

/* 🚀 Custom Button */
.btn-custom {
  background: #ff5722;
  color: white;
  font-weight: bold;
  text-transform: uppercase;
  transition: 0.3s;
  border: none;
  border-radius: 0;
  padding: 12px;
}

.btn-custom:hover {
  background: #e64a19;
  transform: scale(1.05);
}

/* 📎 Sign Up Link */
.link {
  color: #ff5722;
  text-decoration: none;
  font-weight: bold;
}

.link:hover {
  text-decoration: underline;
}

/* 🏀 Phần phải: Hình ảnh sneaker */
.login-image {
  width: 50%;
  height: 100vh;
  background: linear-gradient(135deg, #ff5722, #ff8a50);
  display: flex;
  justify-content: center;
  align-items: center;
}

.login-image img {
  width: 90%;
  border-radius: 10px;
  box-shadow: 0 8px 20px rgba(0, 0, 0, 0.3);
}

/* Thêm media query cho responsive */
@media (max-width: 768px) {
  .login-box {
    flex-direction: column;
  }
  
  .login-form,
  .login-image {
    width: 100%;
  }
  
  .login-image {
    height: 40vh;
  }
}

/* Import Element Plus base styles */
@import 'element-plus/dist/index.css';
</style>
