<template>
  <div class="register-container">
    <div class="register-box">
      <div class="register-form">
        <div class="brand">
          <img src="data:image/png;base64,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" alt="Google" class="logo" />
          <h1 class="title">Create Your Account</h1>
        </div>

        <form @submit.prevent="register">
          <div class="form-group">
            <input
              v-model="name"
              type="text"
              class="form-control"
              placeholder="Name"
              required
              :class="{ 'error': nameError }"
            />
            <span class="error-message" v-if="nameError">{{ nameError }}</span>
          </div>

          <div class="form-group">
            <input
              v-model="email"
              type="email"
              class="form-control"
              placeholder="Email address"
              required
              :class="{ 'error': emailError }"
            />
            <span class="error-message" v-if="emailError">{{ emailError }}</span>
          </div>

          <div class="form-group">
            <div class="password-input">
              <input
                v-model="password"
                :type="showPassword ? 'text' : 'password'"
                class="form-control"
                placeholder="Password"
                required
                :class="{ 'error': passwordError }"
              />
              <button type="button" class="toggle-password" @click="togglePassword">
                <EyeOutlined v-if="!showPassword" />
                <EyeInvisibleOutlined v-else />
              </button>
            </div>
            <span class="error-message" v-if="passwordError">{{ passwordError }}</span>
          </div>

          <div class="form-group">
            <div class="password-input">
              <input
                v-model="confirmPassword"
                :type="showPassword ? 'text' : 'password'"
                class="form-control"
                placeholder="Confirm password"
                required
                :class="{ 'error': confirmPasswordError }"
              />
              <button type="button" class="toggle-password" @click="togglePassword">
                <EyeOutlined v-if="!showPassword" />
                <EyeInvisibleOutlined v-else />
              </button>
            </div>
            <span class="error-message" v-if="confirmPasswordError">{{ confirmPasswordError }}</span>
          </div>

          <div class="form-options">
            <label class="remember-me">
              <input type="checkbox" v-model="agreeTerms" />
              <span>I agree to the Terms and Conditions</span>
            </label>
          </div>
          <span class="error-message" v-if="termsError">{{ termsError }}</span>

          <button type="submit" class="register-account-btn" :disabled="isLoading">
            <template v-if="isLoading">
              <LoadingOutlined class="loading-icon" />
              <span>Signing up...</span>
            </template>
            <template v-else>
              Create Account
            </template>
          </button>

          <p class="terms">
            By signing up, you confirm that you have read and agree to WPY's
            <a href="#" class="link">Terms of Services</a> and
            <a href="#" class="link">Privacy Policy</a>.
          </p>
        </form>

        <div class="register-link">
          Already have an account? <router-link to="/loginPage" class="link">Sign in</router-link>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import { registerUser } from '@/server/auth/register-service';
import { message } from 'ant-design-vue';
import {
  EyeOutlined,
  EyeInvisibleOutlined,
  LoadingOutlined
} from '@ant-design/icons-vue';

// State management
const router = useRouter();
const name = ref('');
const email = ref('');
const password = ref('');
const confirmPassword = ref('');
const showPassword = ref(false);
const nameError = ref('');
const emailError = ref('');
const passwordError = ref('');
const confirmPasswordError = ref('');
const termsError = ref('');
const isLoading = ref(false);
const agreeTerms = ref(false);
const isDarkMode = ref(false);

// Validation
const validateInputs = () => {
  let isValid = true;

  // Validate name
  if (!name.value.trim()) {
    nameError.value = 'Name is required';
    isValid = false;
  } else {
    nameError.value = '';
  }

  // Validate email
  const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
  if (!email.value.trim()) {
    emailError.value = 'Email is required';
    isValid = false;
  } else if (!emailRegex.test(email.value)) {
    emailError.value = 'Please enter a valid email address';
    isValid = false;
  } else {
    emailError.value = '';
  }

  // Validate password
  const passwordRegex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/;
  if (!password.value) {
    passwordError.value = 'Password is required';
    isValid = false;
  } else if (!passwordRegex.test(password.value)) {
    passwordError.value = 'Password must contain at least 8 characters, one uppercase letter, one lowercase letter, one number, and one special character';
    isValid = false;
  } else {
    passwordError.value = '';
  }

  // Validate confirm password
  if (password.value !== confirmPassword.value) {
    confirmPasswordError.value = 'Passwords do not match';
    isValid = false;
  } else {
    confirmPasswordError.value = '';
  }

  // Validate terms
  if (!agreeTerms.value) {
    termsError.value = 'You must agree to the terms and conditions';
    isValid = false;
  } else {
    termsError.value = '';
  }

  return isValid;
};

// UI handlers
const togglePassword = () => {
  showPassword.value = !showPassword.value;
};

const toggleDarkMode = () => {
  isDarkMode.value = !isDarkMode.value;
  document.body.classList.toggle('dark-mode');
};

// Register handler
const register = async () => {
  if (!validateInputs()) return;

  isLoading.value = true;

  try {
    const response = await registerUser(
      name.value,
      email.value,
      password.value
    );

    if (response) {
      message.success('Registration successful! Redirecting to login...');
      setTimeout(() => {
        router.push('/loginPage');
      }, 1000);
    }
  } catch (error) {
    if (error.response) {
      const errorMessage = error.response.data?.message || 'An error occurred. Please try again later.';
      switch (error.response.status) {
        case 400:
          message.error('Invalid data. Please check your input.');
          break;
        case 409:
          message.error('Email already exists. Please use a different email.');
          break;
        case 500:
          message.error('Server error. Please try again later.');
          break;
        default:
          message.error(errorMessage);
      }
    } else if (error.request) {
      message.error('Unable to connect to the server. Please check your internet connection.');
    } else {
      message.error('Registration failed: ' + error.message);
    }
    console.error('Registration error:', error);
  } finally {
    isLoading.value = false;
  }
};
</script>

<style scoped>
.register-container {
  display: flex;
  justify-content: center;
  align-items: center;
  min-height: 100vh;
  background: linear-gradient(135deg, #CCCCCC 0%, #BBBBBB 100%);
  padding: 20px;
  position: relative;
  overflow: hidden;
}

.register-container::before,
.register-container::after {
  content: '';
  position: absolute;
  width: 2000px;
  height: 2000px;
  border-radius: 50%;
  background: linear-gradient(225deg, #AAAAAA 0%, #BBBBBB 100%);
  top: -10%;
  transform: translateY(-50%);
  z-index: 0;
  transition: all 0.8s ease-in-out;
}

.register-container::before {
  right: 48%;
  animation: float 6s ease-in-out infinite;
}

.register-container::after {
  right: 52%;
  animation: float 6s ease-in-out infinite reverse;
}

@keyframes float {
  0% {
    transform: translateY(-50%) rotate(0deg);
  }
  50% {
    transform: translateY(-55%) rotate(180deg);
  }
  100% {
    transform: translateY(-50%) rotate(360deg);
  }
}

.register-box {
  width: 100%;
  max-width: 500px;
  background: rgba(255, 255, 255, 0.95);
  backdrop-filter: blur(10px);
  border-radius: 24px;
  padding: 40px;
  box-shadow: 0 8px 32px rgba(0, 0, 0, 0.1);
  position: relative;
  z-index: 1;
  animation: fadeIn 0.5s ease-out;
  transition: transform 0.3s ease;
  border: 1px solid rgba(255, 255, 255, 0.2);
}

.register-box:hover {
  transform: translateY(-5px);
  box-shadow: 0 12px 40px rgba(0, 0, 0, 0.15);
}

@keyframes fadeIn {
  from {
    opacity: 0;
    transform: translateY(20px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

.brand {
  text-align: center;
  margin-bottom: 20px;
}

.logo {
  height: 48px;
  margin-bottom: 5px;
  margin-top: 5px;
  background-color: transparent;
  transition: transform 0.3s ease;
}

.logo:hover {
  transform: scale(1.05);
}

.title {
  font-size: 28px;
  font-weight: 700;
  color: #111827;
  margin: 0;
  background: linear-gradient(135deg, #555555, #555555);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  text-shadow: 0 2px 4px rgba(85, 85, 85, 0.1);
}

.form-group {
  margin-bottom: 24px;
}

.form-control {
  width: 100%;
  padding: 14px 18px;
  border: 2px solid #E5E7EB;
  border-radius: 12px;
  font-size: 16px;
  transition: all 0.3s ease;
  background: rgba(255, 255, 255, 0.9);
}

.form-control:focus {
  border-color: #555555;
  outline: none;
  box-shadow: 0 0 0 4px rgba(85, 85, 85, 0.1);
  transform: translateY(-2px);
  background: white;
}

.password-input {
  position: relative;
}

.toggle-password {
  position: absolute;
  right: 16px;
  top: 50%;
  transform: translateY(-50%);
  background: none;
  border: none;
  color: #6B7280;
  cursor: pointer;
  padding: 4px;
  transition: all 0.3s ease;
}

.toggle-password:hover {
  color: #555555;
  transform: translateY(-50%) scale(1.1);
}

.register-account-btn {
  width: 100%;
  padding: 16px;
  background: linear-gradient(135deg, #555555, #555555);
  color: white;
  border: none;
  border-radius: 12px;
  font-weight: 600;
  font-size: 16px;
  cursor: pointer;
  transition: all 0.3s ease;
  position: relative;
  overflow: hidden;
  box-shadow: 0 4px 15px rgba(85, 85, 85, 0.2);
}

.register-account-btn:hover {
  transform: translateY(-2px);
  box-shadow: 0 6px 20px rgba(85, 85, 85, 0.3);
  background: linear-gradient(135deg, #555555, #555555);
}

.register-account-btn:active {
  transform: translateY(0);
  box-shadow: 0 2px 10px rgba(85, 85, 85, 0.2);
}

.register-account-btn::after {
  content: '';
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background: linear-gradient(135deg, rgba(255,255,255,0.1), rgba(255,255,255,0));
  opacity: 0;
  transition: opacity 0.3s ease;
}

.register-account-btn:hover::after {
  opacity: 1;
}

.terms {
  text-align: center;
  font-size: 14px;
  color: #6B7280;
  margin: 25px 0;
  line-height: 1.6;
}

.link {
  color: #555555;
  text-decoration: none;
  font-weight: 500;
  transition: all 0.3s ease;
  position: relative;
}

.link:hover {
  color: #555555;
}

.link::after {
  content: '';
  position: absolute;
  width: 100%;
  height: 2px;
  bottom: -2px;
  left: 0;
  background: linear-gradient(to right, #555555, #555555);
  transform: scaleX(0);
  transition: transform 0.3s ease;
}

.link:hover::after {
  transform: scaleX(1);
}

.register-link {
  text-align: center;
  font-size: 14px;
  color: #6B7280;
  margin-top: 24px;
}

.loading-icon {
  animation: spin 1s linear infinite;
  margin-right: 8px;
}

@keyframes spin {
  from {
    transform: rotate(0deg);
  }
  to {
    transform: rotate(360deg);
  }
}

.register-account-btn:disabled {
  opacity: 0.7;
  cursor: not-allowed;
  transform: none !important;
}

.form-control.error {
  border-color: #EF4444;
  background-color: rgba(239, 68, 68, 0.05);
}

.error-message {
  color: #EF4444;
  font-size: 14px;
  margin-top: 4px;
  display: block;
}

.form-options {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 24px;
}

.remember-me {
  display: flex;
  align-items: center;
  gap: 8px;
  cursor: pointer;
  margin-left: 5px;
}

.remember-me input[type="checkbox"] {
  width: 16px;
  height:16px;
  margin-left: 5px;
  cursor: pointer;
}

/* Dark mode styles */
.dark-mode .register-container {
  background: linear-gradient(135deg, #333333 0%, #222222 100%);
}

.dark-mode .register-box {
  background: rgba(0, 0, 0, 0.8);
  border-color: rgba(255, 255, 255, 0.1);
}

.dark-mode .form-control {
  background: rgba(255, 255, 255, 0.05);
  border-color: rgba(255, 255, 255, 0.1);
  color: #ffffff;
}

.dark-mode .form-control:focus {
  background: rgba(255, 255, 255, 0.1);
}

.dark-mode .title {
  color: #ffffff;
}

.dark-mode .terms {
  color: #AAAAAA;
}

/* Responsive styles */
@media (max-width: 480px) {
  .form-options {
    flex-direction: column;
    align-items: flex-start;
    gap: 12px;
  }
  
  .register-box {
    padding: 20px;
  }
  
  .title {
    font-size: 22px;
  }
  
  .form-control {
    padding: 10px 14px;
    font-size: 14px;
  }
}
</style>