<template>
    <div class="verify-container">
      <div class="verify-box">
        <div class="verify-form">
          <div class="brand">
            <img src="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAOEAAADhCAMAAAAJbSJIAAABX1BMVEX////qQzU0qFNChfT7vAUufPPg6P07gfSGrfc1f/T7uQCxyPowp1DqQTPqPzD/vQDpNyYnpUrpOirpNCP5zsvpMR4do0X5zcr8vwBDg/r++Pfz+vUqpUz97Ov98fDpKxXpOzfF5Mzzn5nyl5Hucmn2trH62tjxiYLsWU774d/rTUD1rqmhv/n0+P5ArV2n1bJguHbo9euc0am53sIzqkbT69n4w7/ykYvtYVfvdWzwgHn0qKPsUDH/+/D+78r80W395KjzhSD/+OP8zVf93ZP94qL8x0j+5rTE1/v80Gj814hNqkt0wIeBq/dRjvVNsmj4t3Hvbyj3pBPygSH8xTTwcyb0jxz4qBDtXC3xf0donfbW5PzH2PuRuVvduRe2tCiCrz2UsDhjrEXStxyrszDIyW5imPY8j8Y6mZ+LyZo3oHs/jtE8lq85nYw2pGdBieE/i9o9k7s5nJQ2o29dpb2Lsm4PAAALPklEQVR4nO2c63vayBXGhQx2TCQhCaQlgPECxuYSczU2NpukdXa7aUpCm253e0kv2916y3Yvvf3/TyVhZIQ00sxIMyOp7wc/efwB6edz5rznzAzhuJ122mmnnXbaaadIVCiXm7nL+qRardZqxo/JpH6Zax6VC6xfLAKVm5fVs5OGqGlaVpZldSHjX1njN2rj5KZWPz3aVNDyaftsnDDAVEkSE14SRcmg1aTGde2DJuvXRVQhVz1pmWzeaGugkqQamDeXG0PZbJ+0IOFWMdWs1LjKsX75YDUnDVVGpHNQ3nzAGsFPR+3WMS6eTalpZ6esQbxVuLw28MLQLSFVrTUps8Zx6WjS0qLAu4eU5ZgF8vRMkkMlp5tR1U7isyJz17IaLZ8lKTu+ZI1m6fQkS4IvNozN65DFM5CR7Xosn2mk4mczatdH7AAnxPlMqcdVRny5RpYCX8L0jhaLsmokaHT+FyRJO6PeAly2ZGp8pmiHsXCTpRfAhST5iuKknGuplPkS1mqkNkDWKK7AVUnZOhW+8phSCXVL1G4oAOYSbAK4kDwmbv/1SEZAfEkJwhsdVxqrDLURVZKLsXCdZcyXMBfjhBhgucHAJNwStRohwKMW2yVoSzwmM1E1mRbRFYnHZIrNqRQXwOy2A6rbDiiRAWyKsQEkM0QdxabIEErRcmxsglCR4caxMHpyNsFd092vAErUCAHWYtCLmhJlQoB15tPEQqSKDHdKY88XQqRsgitEVkbN+wjmNRN5cekE9ZhfJbWZeBJFGRVVWTsWx9dX1Um7XW+3J9Wrm3FL07LQnMTWIDfRwtNlj1tn7Zx7y7pwWq+NNQ1mV4RYFeVOQ1YZA088u/Tdj89VG9mg8zliPsgVGmEWoZGbrRrMqHpUH6t+9YxYJ8NxVyGsXpSlG/gXa9ZawGMCYjZhJBD+5QMx26qibWwW6mNvRlLjEhcmRw2+NsZh2KXXZjoxHzRUw81RVWxjnhLVXUd2BFOUa2LmqHR8hX+YWagdOxKHYJExRiasHBWzjXBHYM3Vo3NyNsGZDTdWAOXw+9FVu+KQM3pTOJdkRHkcxRnm8vyVXKtmqopRZsTsVTQPL5+YIynJImM8A+MQNMpDoZomkrQJQ1foI4XUivIwoX1MbFyy1EQfe9VGtIezl2Qvl/wSOYTyeKO+NHGbT3/0IRrgCet3RtOLdD7/KxRE+Zr1K6PpNmko/zk8oLphEeRepy3Ed7CZKm3WGuS4Z8mF8mm4TJVa8fvmgL8+SSeXjL+GQBTVjfm60lKfJW3l3wX7osb+rjminqeTK4iBtpFldUMZXy9WCZP5/G98ETeujBp1xgEYZBtiguE9ekz9ZJ0wmf8CmKmivHGL0FFnHhYjyDZUGtc9I9atK4QW4089EUVp83J0xQydiO8SHozZNuvXxZBHkt5n6m9diFKL9dtiyDtJLcSkyzZIbvUR06dAQrdtSJtnhYZe+hCu24YWr6+xwumZD19yzTakDZt6F3ruF0KL8cE2iO5HE9PHQYQPtiGNWb8sll4GAT7YRnYD+7XAZXiPaNmGuHGDvSWwGzoZP/8woUZ0REFZgJbNjfjFR/ImWsX68OuHmP4d63fF088gAZPJ9Gvsh5zvE9Y5+NnP8vCEz7EJH6cI6/fgZ0MWGkvYgNzjwz3CAj/br+1eC+HPY0yYAj/7NTzhp3Em3Ac+G7qUJtO3MSY8fAV8NkTPthQ+IAXCx6BHPwPtYLhD+DLOhAePgITQZpH+JNaEQLtw7XaDCUMUGgqEd6BHw9thmEJDwQ+Bhhg44D8QhgCkQQjq29wnFiDl4014ACKEnZ2Syc9iTgiy/OBNmmWShujZaBAe/iE0If7oRIfwPeDR0G1p+uOtJwxj+DQIQY3pjnBzCEGt947w/4kw7rUURLg9fhieMO49DYhwa/pSoFvAbybGfLYAEm7NfAjs2rZmxgcSbss+DXh62pa9NvAEvC37peBdjG3Z8wbvRG3LuYXP8dqWnD0dvAU+e0vOD8G7+ttyBgw+maF1js/udI3SXQyWJ6TwxTST/CM+YeoAS9CEKVBLw8HPT5k//aI0wiV8/+YRjt7AE/o8HLLUZP4s8MoclxBT5/DJ7fMpUHcTM8m/8DwvdIrU4Cy9giX0MQsOqm8zMpQ3pVdosS30CHYhHjz1+5jgrRozQy0JPVpsC93BEvqV0uAxP5P8kl9Kx641ONpPQQL6XRjiAhdiJv9XG5AvTWnRmUJwUf8P8v2+ReYrgV8R1SDewfL5Fxr/5jvzNe8QzSDCJ+mhb6Hxc8SFSfCMgvgUuqVJgbahlgLtZCxNYlXCjAqdKfie7dDnirAlQONmm4QziEMqeAh2v7cXsAwBaZrJfOnBZwSRp9TYvIWOYdAy5DzT1GESDil0ig1CCP0Gi3u5b0ZlvnIvQRuRSu8GbRV7e2+DlqHHznfma68luMxTGg04gtsfvIH4POcY7GESDlEwxXP4CO6lfJvSezl6Uy+TcErvkiaEnipMBScp56g1a32aNyJh33+PsK8T1LLdy641IJNYW4p8nyTgObxTBE1OtpYDBtgknCrNSFabNyg5Cpeky919P5NYQyQ4DD+Fbrn3ICupqdtAk1gTOeN/hQIIPvx16UU6k/E3CUqI+2i7x+AjmXXdBpvEmnQiiPtIfDA9qa2/ldAAyUQRETB4cFrRSEEl5JVe1BUVFRC6zliaIgfRMI1offE9ImDAJtu6+joyIS8IUQ4ajxHOYhaC62dszdHzlBeUQWSAj5DP4AI3aNZUxCA0F2M0mbp/h37ICPw+F0hdjDw1FqMQxdbNqyeoGYoeQkMz+J5mVXroMPZ7+jdPUAEhpwqHRlhBNMKoh1uNA6XEX/wdtc7AN2wrwik2lpQOfqoOO9ZTL75FGZrwQmgUmw5enhpFVZ9hMRaHM11Y/pX+gZKpB8FbbF6q4AbRYuyi9jjFrs1nfoLwHTyizxUaf2HnqfmGSmeOsr8xmvKKM2cuvt+DzlSEjtQh/Dy1GEvKrAtXWEfzjuLuE5UffoQLo98doaAnY9ZTG1LRO4OASPaHU0FRPP+UwgWcbSCb/YoGIREtSL03GHrGsl/pTgXdI3q2oGwDreVeVw99yPCiNDBn0/mgOxxWKpXhsDsYzHsz45d+dAvEb38MQkQZfD0Ubik6MEslxZRu/VRKAtwHB9oG+D8YgBTGMBytBOU73ygC761Daxh6KYbVxfc+DU4Kv47aCuOK0cjHNg6Rti5A6jFHFJR/ARAhzgthNIuioIbTxb+9G5zQi3Ch6ApqCMT/eNgG5EkMhPo8e0Sl42pwUuGc0IkI6V4kJVys2UY0VWapUQwQ12zjEGvqjTmi8sM/7UwN3cu4EUsxQBSEpW0cROQTDsQYlBvTNqxpgwSgUW467H3Rso0nhAANX5wx724MlfhvUndkALk4NHC8aRv/JQZotOHMJw0SR5UOdb23VChKJ/11nRHPtt6EPDSAUZHlYhTo3Pec66wyVYn4KB2oSodJGAUyl1o8VZwyCGM0x6/QGlIPY/izV0RRDmNJIH5V160RvSZOoB7Aew10Ot6o0F2BqyrOKTCWiHcxvhpNA09XQvIpUzYJusLYI8ho8FH9ripAxOIYEz5To7kQ+cwhKKU56/xcVX/QibTolPTZIE58lipTPaJACoo+pfx9f0j1u7PwkAYe+mUciuoPZoqCvXsslBToKyoM1e/2OhiUJl1n6n1rI34qjga9jq7AbpMbcLowmw5HMU5ODxVH3XmPN2+UAMMpCGbgTLh5pb9ZdLaK/cpgPp0ZoPe3TGyZv+Fnvflg2N9UuBUVi8X+yLooNBjMzR+D7rAy6hc3n2ynnXbaaad/0P8Aq3nyeHUSpxEAAAAASUVORK5CYII=" alt="Google" class="logo" />
            <h1 class="title">Verify Your Email</h1>
          </div>
  
          <form @submit.prevent="verifyOTP">
            <div class="form-group">
              <input
                v-model="otp"
                type="text"
                class="form-control"
                placeholder="Enter OTP code"
                required
                :class="{ 'error': otpError }"
              />
              <span class="error-message" v-if="otpError">{{ otpError }}</span>
            </div>
  
            <button type="submit" class="verify-btn" :disabled="isLoading">
              <template v-if="isLoading">
                <LoadingOutlined class="loading-icon" />
                <span>Verifying...</span>
              </template>
              <template v-else>
                Verify OTP
              </template>
            </button>
  
            <div class="resend-section">
              <p>Didn't receive the code?</p>
              <button type="button" class="resend-btn" @click="resendOTP" :disabled="isResending">
                {{ isResending ? `Resend in ${countdown}s` : 'Resend OTP' }}
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>
  </template>
  
  <script setup>
  import { ref, onMounted } from 'vue';
  import { useRouter, useRoute } from 'vue-router';
  import { message } from 'ant-design-vue';
  import { LoadingOutlined } from '@ant-design/icons-vue';
  import { verifyOTP as verifyOTPService, registerUser } from '@/server/auth/register-service';
  
  const router = useRouter();
  const route = useRoute();
  const otp = ref('');
  const otpError = ref('');
  const isLoading = ref(false);
  const isResending = ref(false);
  const countdown = ref(60);
  
  // Get email from route query
  const email = ref(route.query.email || '');
  
  // Verify OTP
  const verifyOTP = async () => {
    if (!otp.value) {
      otpError.value = 'Please enter OTP code';
      return;
    }
  
    isLoading.value = true;
    try {
      // Verify OTP
      await verifyOTPService(email.value, otp.value, "ResetPassword");
      console.log("verifyOTP", email.value, otp.value, "ResetPassword");
      message.success('Email verified successfully! Redirecting to login...');
      setTimeout(() => {
        router.push('/forgotPassword');
      }, 1000);
    } catch (error) {
      otpError.value = 'Invalid OTP code';
      message.error('Verification failed. Please try again.');
    } finally {
      isLoading.value = false;
    }
  };
  
  // Resend OTP
  const resendOTP = async () => {
    if (isResending.value) return;
  
    isResending.value = true;
    countdown.value = 60;
  
    try {
      await verifyOTPService(email.value, '', "register");
      message.success('OTP sent successfully!');
      
      // Start countdown
      const timer = setInterval(() => {
        countdown.value--;
        if (countdown.value <= 0) {
          clearInterval(timer);
          isResending.value = false;
        }
      }, 1000);
    } catch (error) {
      message.error('Failed to resend OTP. Please try again.');
      isResending.value = false;
    }
  };
  
  onMounted(() => {
    if (!email.value) {
      router.push('/registerPage');
    }
  });
  </script>
  
  <style scoped>
  .verify-container {
    display: flex;
    justify-content: center;
    align-items: center;
    min-height: 100vh;
    background: linear-gradient(135deg, #f5f7fa 0%, #e4e9f2 100%);
    padding: 20px;
  }
  
  .verify-box {
    width: 100%;
    max-width: 400px;
    background: rgba(255, 255, 255, 0.95);
    backdrop-filter: blur(10px);
    border-radius: 24px;
    padding: 40px;
    box-shadow: 0 8px 32px rgba(0, 0, 0, 0.1);
    animation: fadeIn 0.5s ease-out;
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
    margin-bottom: 30px;
  }
  
  .logo {
    height: 48px;
    margin-bottom: 10px;
  }
  
  .title {
    font-size: 24px;
    font-weight: 600;
    color: #333;
    margin: 0;
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
  }
  
  .form-control:focus {
    border-color: #AAAAAA;
    outline: none;
    box-shadow: 0 0 0 4px rgba(170, 170, 170, 0.1);
  }
  
  .verify-btn {
    width: 100%;
    padding: 16px;
    background: #AAAAAA;
    color: white;
    border: none;
    border-radius: 12px;
    font-weight: 600;
    font-size: 16px;
    cursor: pointer;
    transition: all 0.3s ease;
  }
  
  .verify-btn:hover:not(:disabled) {
    background: #888888;
    transform: translateY(-2px);
  }
  
  .verify-btn:disabled {
    opacity: 0.7;
    cursor: not-allowed;
  }
  
  .resend-section {
    text-align: center;
    margin-top: 24px;
  }
  
  .resend-section p {
    color: #666;
    margin-bottom: 8px;
  }
  
  .resend-btn {
    background: none;
    border: none;
    color: #AAAAAA;
    font-weight: 500;
    cursor: pointer;
    transition: color 0.3s ease;
  }
  
  .resend-btn:hover:not(:disabled) {
    color: #888888;
  }
  
  .resend-btn:disabled {
    color: #999;
    cursor: not-allowed;
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
  
  .error-message {
    color: #EF4444;
    font-size: 14px;
    margin-top: 4px;
    display: block;
  }
  
  .form-control.error {
    border-color: #EF4444;
    background-color: rgba(239, 68, 68, 0.05);
  }
  </style> 