<!-- Header Component - Flexbox Optimized Sneaker Theme -->
<template>
  <header class="sneaker-header" :class="{ 'scrolled': isScrolled }">
    <div class="header-container">
      <!-- Logo Section -->
      <router-link to="/homePage" class="logo-wrapper">
        <img src="data:image/jpeg;base64,/9j/..." 
             alt="Sneaker Logo" 
             class="logo-img" 
             @mouseover="animateLogo"
             @mouseleave="resetLogo" />
        <span class="brand-text">SneakerVibe</span>
      </router-link>

      <!-- Navigation -->
      <nav class="navbar-nav" :class="{ 'nav-open': isNavOpen }">
        <router-link v-for="item in navItems" 
                    :key="item.path" 
                    :to="item.path" 
                    class="nav-link"
                    :class="{ 'highlight-link': item.highlight }"
                    @click="toggleNav">
          <a-icon :type="item.icon" /> {{ item.label }}
        </router-link>
      </nav>

      <!-- User Actions -->
      <div class="user-actions">
        <div class="search-wrapper" :class="{ 'expanded': isSearchOpen }">
          <a-input-search v-if="isSearchOpen"
                         v-model="searchQuery"
                         placeholder="Search sneakers..."
                         @search="onSearch"
                         class="search-input" />
          <a-button class="sneaker-btn-icon search-btn"
                   shape="circle"
                   @click="toggleSearch">
            <a-icon type="search" />
          </a-button>
        </div>

        <router-link to="/cartPage" class="sneaker-btn-icon cart-btn">
          <a-icon type="shopping-cart" />
          <span class="sneaker-badge">{{ cartCount }}</span>
        </router-link>

        <a-dropdown v-if="isAuthenticated" class="user-dropdown">
          <a-button class="sneaker-btn user-btn">
            <a-icon type="user" /> {{ userName }}
            <a-icon type="down" />
          </a-button>
          <a-dropdown-menu slot="overlay">
            <a-dropdown-item><a-icon type="profile" /> Profile</a-dropdown-item>
            <a-dropdown-item><a-icon type="setting" /> Settings</a-dropdown-item>
            <a-dropdown-item @click="logout"><a-icon type="logout" /> Logout</a-dropdown-item>
          </a-dropdown-menu>
        </a-dropdown>
        <router-link v-else to="/loginPage" class="sneaker-btn">
          <a-icon type="user" /> Login
        </router-link>

        <a-button class="sneaker-btn-icon theme-btn"
                 shape="circle"
                 @click="toggleTheme">
          <a-icon :type="isDarkTheme ? 'sun' : 'moon'" />
        </a-button>
      </div>

      <!-- Mobile Toggle -->
      <button class="navbar-toggler" @click="toggleNav">
        <span class="toggler-icon"></span>
      </button>
    </div>
  </header>
</template>

<script setup>
import { ref, onMounted, onUnmounted } from 'vue';

const isAuthenticated = ref(false);
const userName = ref('SneakerFan');
const cartCount = ref(2);
const isNavOpen = ref(false);
const isSearchOpen = ref(false);
const searchQuery = ref('');
const isDarkTheme = ref(true);
const isScrolled = ref(false); // Thêm biến trạng thái scroll

const navItems = [
  { path: '/homePage', label: 'Home', icon: 'home' },
  { path: '/aboutPage', label: 'About', icon: 'info-circle' },
  { path: '/productPage', label: 'Products', icon: 'shop' },
  { path: '/contactPage', label: 'Contact', icon: 'mail' },
  { path: '/customPage', label: 'Customize', icon: 'edit', highlight: true },
];

// Xử lý sự kiện scroll
const handleScroll = () => {
  isScrolled.value = window.scrollY > 50; // Thay đổi trạng thái khi scroll quá 50px
};

onMounted(() => {
  window.addEventListener('scroll', handleScroll);
});

onUnmounted(() => {
  window.removeEventListener('scroll', handleScroll);
});

const toggleNav = () => {
  isNavOpen.value = !isNavOpen.value;
  if (isSearchOpen.value) isSearchOpen.value = false;
};

const toggleSearch = () => {
  isSearchOpen.value = !isSearchOpen.value;
  if (isSearchOpen.value) searchQuery.value = '';
};

const onSearch = (value) => {
  console.log('Searching for:', value);
  isSearchOpen.value = false;
};

const logout = () => {
  isAuthenticated.value = false;
};

const toggleTheme = () => {
  isDarkTheme.value = !isDarkTheme.value;
  document.body.classList.toggle('light-theme', !isDarkTheme.value);
};

const animateLogo = (e) => {
  e.target.style.transform = 'rotate(360deg) scale(1.1)';
};

const resetLogo = (e) => {
  e.target.style.transform = 'rotate(0deg) scale(1)';
};
</script>

<style scoped>
.sneaker-header {
  background: #1a1a1a;
  padding: 1rem 0;
  box-shadow: 0 4px 20px rgba(0, 0, 0, 0.3);
  position: sticky;
  top: 0;
  z-index: 1000;
  transition: all 0.3s ease; /* Thêm transition mượt mà */
}

.sneaker-header.scrolled {
  padding: 0.5rem 0; /* Thu nhỏ padding khi scroll */
  background: rgba(26, 26, 26, 0.9); /* Đổi màu nền nhẹ trong suốt */
  box-shadow: 0 6px 25px rgba(0, 0, 0, 0.5); /* Tăng bóng */
}

.sneaker-header.scrolled .logo-img {
  width: 35px; /* Thu nhỏ logo */
  height: 35px;
}

.sneaker-header.scrolled .brand-text {
  font-size: 1.5rem; /* Thu nhỏ chữ */
}

.header-container {
  display: flex;
  flex-wrap: wrap;
  justify-content: space-between;
  align-items: center;
  max-width: 1200px;
  margin: 0 auto;
  padding: 0 1rem;
}

/* Các style khác giữ nguyên */
.logo-wrapper {
  display: flex;
  align-items: center;
  text-decoration: none;
}

.logo-img {
  width: 45px;
  height: 45px;
  transition: transform 0.5s ease, width 0.3s ease, height 0.3s ease; /* Thêm transition cho logo */
  margin-right: 12px;
}

.brand-text {
  font-size: 1.8rem;
  font-weight: 700;
  color: #8bc34a;
  letter-spacing: 1px;
  background: linear-gradient(45deg, #8bc34a, #ff6200);
  -webkit-background-clip: text;
  background-clip: text;
  -webkit-text-fill-color: transparent;
  transition: font-size 0.3s ease; /* Thêm transition cho chữ */
}

/* Các style khác giữ nguyên */
.navbar-nav {
  display: flex;
  flex-direction: row;
  gap: 2rem;
  align-items: center;
}

.nav-link {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  color: #fff;
  font-weight: 600;
  text-decoration: none;
  padding: 0.5rem 1rem;
  border-radius: 8px;
  transition: all 0.3s ease;
}

/* Các style khác giữ nguyên */
@media (max-width: 991px) {
  .header-container {
    flex-direction: row;
    align-items: center;
  }

  .navbar-nav {
    display: none;
    flex-direction: column;
    width: 100%;
    background: #1a1a1a;
    padding: 1rem;
    box-shadow: 0 4px 10px rgba(0, 0, 0, 0.2);
  }

  .sneaker-header.scrolled .navbar-nav {
    background: rgba(26, 26, 26, 0.9); /* Đảm bảo đồng bộ màu khi scroll */
  }

  .nav-open {
    display: flex;
  }

  /* Các style khác giữ nguyên */
}

body.light-theme .sneaker-header {
  background: #ffffff;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
}

body.light-theme .sneaker-header.scrolled {
  background: rgba(255, 255, 255, 0.9); /* Đổi màu nền khi scroll ở light theme */
  box-shadow: 0 6px 25px rgba(0, 0, 0, 0.2);
}
</style>