<template>
  <div class="not-authorized-container">
    <div class="not-authorized-content">
      <h1 class="not-authorized-title">Access Denied</h1>
      <p class="not-authorized-message">You do not have permission to access this page.</p>
      <button class="back-home-btn" @click="goBack">Go Back</button>
    </div>
  </div>
</template>

<script setup>
const router = useRouter();

function goBack() {
  let role = '';
  if (process.client) {
    role = (localStorage.getItem('role') || '').toLowerCase();
  }
  if (role === 'admin') {
    router.push('/admin');
  } else if (role === 'manufacturer') {
    router.push('/manufacturer');
  } else if (window.history.length > 1) {
    router.back();
  } else {
    router.push('/');
  }
}
</script>

<style scoped>
.not-authorized-container {
  min-height: 60vh;
  display: flex;
  align-items: center;
  justify-content: center;
  background: linear-gradient(135deg, #f8fafc 0%, #e0e7ef 100%);
}
.not-authorized-content {
  background: #fff;
  padding: 2.5rem 3rem;
  border-radius: 1.25rem;
  box-shadow: 0 4px 32px rgba(0,0,0,0.08);
  text-align: center;
}
.not-authorized-title {
  font-size: 2.5rem;
  font-weight: 700;
  color: #3b82f6;
  margin-bottom: 1rem;
}
.not-authorized-message {
  font-size: 1.2rem;
  color: #555;
  margin-bottom: 2rem;
}
.back-home-btn {
  display: inline-block;
  padding: 0.75rem 2rem;
  background: #3b82f6;
  color: #fff;
  border-radius: 2rem;
  font-weight: 600;
  text-decoration: none;
  transition: background 0.2s;
}
.back-home-btn:hover {
  background: #2563eb;
}
</style>