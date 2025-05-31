<template>
  <header class="sneaker-header" :class="{ 'scrolled': isScrolled }">
    <div class="header-container">
      <!-- Logo Section -->
      <a href="/manufacturer" class="logo-wrapper">
        <img 
          src="https://th.bing.com/th/id/OIP.EL5hPJ7k0B7W_D7EbZoexgHaEd?w=338&h=180&c=7&r=0&o=5&dpr=1.3&pid=1.7" 
          alt="Sneaker Logo" 
          class="logo-img" 
          @mouseover="animateLogo"
          @mouseleave="resetLogo" 
        />
        <span class="brand-text">WPY</span>
      </a>

      <!-- Navigation -->
      <nav class="navbar-nav" :class="{ 'nav-open': isNavOpen }">
        <template v-for="item in navItems" :key="item.path || item.label">
          <a 
            v-if="item.path && item.path !== '/customPage'" 
            :href="item.path" 
            class="nav-link"
            @click="toggleNav"
          >
            <a-icon :type="item.icon" /> {{ item.label }}
          </a>
          
          <div v-else-if="item.path === '/customPage'" class="custom-dropdown">
            <div class="nav-link" style="cursor: pointer;">
              <a-icon :type="item.icon" /> {{ item.label }}
              <DownOutlined style="margin-left: 5px; font-size: 12px;" />
            </div>
            <div class="dropdown-content">
              <a href="/customPage" class="dropdown-item" @click="toggleNav">
                <ShoppingOutlined style="margin-right: 8px;" /> Product Design
              </a>
              <a href="/mycustomPage" class="dropdown-item" @click="toggleNav">
                <UserOutlined style="margin-right: 8px;" /> My Designs
              </a>
            </div>
          </div>

          <div v-else-if="item.subItems" class="custom-dropdown">
            <div class="nav-link" style="cursor: pointer;">
              <a-icon :type="item.icon" /> {{ item.label }}
              <DownOutlined style="margin-left: 5px; font-size: 12px;" />
            </div>
            <div class="dropdown-content">
              <a 
                v-for="subItem in item.subItems" 
                :key="subItem.path"
                :href="subItem.path" 
                class="dropdown-item" 
                @click="toggleNav"
              >
                <a-icon :type="subItem.icon" style="margin-right: 8px;" /> {{ subItem.label }}
              </a>
            </div>
          </div>
        </template>
      </nav>

      <!-- User Actions -->
      <div class="user-actions">
        <!-- User Section -->
        <section v-if="isAuthenticated">
          <div class="user-dropdown">
            <div class="nav-link" style="cursor: pointer;">
              <i class="bi bi-person-circle me-1"></i> {{ userName }}
            </div>
            <ul class="dropdown-content" aria-labelledby="userDropdown">
              <li>
                <a class="dropdown-item" href="/profileManuPage">
                  <i class="bi bi-person me-2"></i> Profile
                </a>
              </li>
              <li><hr class="dropdown-divider"></li>
              <li>
                <a class="dropdown-item text-danger" href="#" @click.prevent="emitLogout">
                  <i class="bi bi-box-arrow-right me-2"></i> Logout
                </a>
              </li>
            </ul>
          </div>
        </section>
        
        <!-- Login Link (Non-Authenticated) -->
        <section v-else>
          <a href="/loginPage" class="login-btn">
            <UserOutlined /> Login
          </a>
        </section>
      </div>

      <!-- Mobile Toggle Button -->
      <button class="navbar-toggler" @click="toggleNav">
        <span class="toggler-icon"></span>
      </button>
    </div>
  </header>
</template>

<script setup>
import { ref, onMounted, onUnmounted, computed, watch, nextTick } from 'vue';
import { 
  ShoppingCartOutlined, 
  UserOutlined, 
  DownOutlined, 
  ProfileOutlined, 
  SettingOutlined, 
  LogoutOutlined, 
  HeartOutlined,
  BellOutlined,
  ShoppingOutlined
} from '@ant-design/icons-vue';
import { useCart } from '~/composables/useCart';

const { cartCount } = useCart();
const isAuthenticated = ref(false);
const userName = ref('SneakerFan');
const userRole = ref('');
const isNavOpen = ref(false);
const isSearchOpen = ref(false);
const searchQuery = ref('');
const isDarkTheme = ref(true);
const isScrolled = ref(false);

const emit = defineEmits(['logout']); // Define the logout event

const navItems = computed(() => {
  const items = [
    { path: '/manufacturer', label: 'Home', icon: 'home' },
  ];
  if (isAuthenticated.value && userRole.value === 'Manufacturer') {
    items.push(
      { path: '/manufacturer/manu_Manage_Order', label: 'Order Management', icon: 'shopping' },
      { path: '/manufacturer/manageService', label: 'Service Management', icon: 'setting' }
    );
  }
  return items;
});

// Watch for authentication state changes
watch(() => {
  const token = localStorage.getItem('userToken');
  const role = localStorage.getItem('role');
  isAuthenticated.value = !!token;
  userRole.value = role || '';
  if (isAuthenticated.value) {
    userName.value = localStorage.getItem('username') || 'User';
  }
}, { immediate: true });

// Handle scroll event
const handleScroll = () => {
  isScrolled.value = window.scrollY > 50;
};

const initDropdowns = async () => {
  if (process.client) {
    try {
      const bootstrap = await import('bootstrap/dist/js/bootstrap.bundle.min.js');
      const dropdownElements = document.querySelectorAll('.dropdown-toggle');
      dropdownElements.forEach(element => {
        new bootstrap.Dropdown(element);
      });
    } catch (error) {
      console.error('Error initializing dropdowns:', error);
    }
  }
};

onMounted(() => {
  window.addEventListener('scroll', handleScroll);
  nextTick(() => {
    initDropdowns();
  });
  const token = localStorage.getItem('userToken');
  userName.value = localStorage.getItem('username') || 'User';
  userRole.value = localStorage.getItem('role') || '';
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

const emitLogout = () => {
  // Emit logout event for parent components to handle
  emit('logout');
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
  background: rgba(255, 255, 255, 0.95);
  backdrop-filter: blur(10px);
  padding: 1rem 0;
  box-shadow: 0 4px 20px rgba(0, 0, 0, 0.1);
  position: sticky;
  top: 0;
  z-index: 1000;
  transition: all 0.3s ease;
}

.sneaker-header.scrolled {
  padding: 0.5rem 0;
  background: rgba(255, 255, 255, 0.9);
  box-shadow: 0 6px 25px rgba(0, 0, 0, 0.1);
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

.logo-wrapper {
  display: flex;
  align-items: center;
  text-decoration: none;
  transition: transform 0.3s ease;
}

.logo-wrapper:hover {
  transform: scale(1.05);
}

.logo-img {
  width: 45px;
  height: 45px;
  transition: all 0.3s ease;
  margin-right: 12px;
  border-radius: 50%;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
}

.brand-text {
  font-size: 1.8rem;
  font-weight: 700;
  color: #555555;
  letter-spacing: 1px;
  transition: all 0.3s ease;
}

.navbar-nav {
  display: flex;
  flex-direction: row;
  gap: 1.5rem;
  align-items: center;
}

.nav-link {
  display: flex;
  align-items: center;
  color: #555555;
  font-weight: 600;
  text-decoration: none;
  padding: 0.5rem 1rem;
  border-radius: 10px;
  transition: all 0.3s ease;
  position: relative;
}

.nav-link::after {
  content: '';
  position: absolute;
  bottom: 0;
  left: 50%;
  width: 0;
  height: 2px;
  background: #555555;
  transition: all 0.3s ease;
  transform: translateX(-50%);
}

.nav-link:hover::after {
  width: 80%;
}

.nav-link:hover {
  transform: none;
  box-shadow: none;
  background: none;
}

.user-actions {
  display: flex;
  align-items: center;
  gap: 1rem;
}

.sneaker-btn-icon {
  position: relative;
  background: linear-gradient(135deg, #555555, #555555);
  color: #fff;
  border-radius: 24px;
  padding: 0.5rem;
  transition: all 0.3s ease;
}

.sneaker-btn-icon:hover {
  transform: translateY(-2px);
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
  background: linear-gradient(135deg, #444444, #444444);
}

.sneaker-badge {
  position: absolute;
  top: -5px;
  right: -5px;
  background: #555555;
  color: #fff;
  border-radius: 50%;
  padding: 0.2rem 0.5rem;
  font-size: 0.75rem;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
}

.dropdown,
.custom-dropdown {
  position: relative;
  display: inline-block;
}

.dropdown-menu,
.dropdown-content {
  position: absolute;
  top: 100%;
  left: 0;
  display: none;
  background: rgba(255, 255, 255, 0.98);
  min-width: 180px;
  box-shadow: 0 8px 16px rgba(0, 0, 0, 0.1);
  z-index: 1;
  border-radius: 10px;
  padding: 8px 0;
  margin-top: 10px;
  opacity: 0;
  visibility: hidden;
  transition: all 0.3s ease;
}

.dropdown-menu::before,
.dropdown-content::before {
  content: '';
  position: absolute;
  top: -10px;
  left: 0;
  width: 100%;
  height: 10px;
  background: transparent;
}

.dropdown-item {
  color: #555555;
  text-decoration: none;
  display: flex;
  align-items: center;
  padding: 10px 16px;
  transition: all 0.3s ease;
  font-weight: 500;
}

.dropdown-item:hover {
  background-color: rgba(85, 85, 85, 0.08);
  transform: translateX(3px);
}

.dropdown:hover .dropdown-menu,
.custom-dropdown:hover .dropdown-content {
  display: block;
  opacity: 1;
  visibility: visible;
}

@media (max-width: 991px) {
  .header-container {
    flex-direction: row;
    align-items: center;
  }

  .navbar-nav {
    display: none;
    flex-direction: column;
    width: 100%;
    background: rgba(255, 255, 255, 0.95);
    backdrop-filter: blur(10px);
    padding: 1rem;
    box-shadow: 0 4px 10px rgba(0, 0, 0, 0.1);
    border-radius: 24px;
  }

  .nav-open {
    display: flex;
  }

  .user-actions {
    gap: 0.75rem;
  }

  .dropdown-menu,
  .dropdown-content {
    position: static;
    display: none;
    box-shadow: none;
    margin-top: 0;
    padding-left: 1rem;
    opacity: 1;
    visibility: visible;
    transition: none;
  }
  
  .dropdown:hover .dropdown-menu,
  .custom-dropdown:hover .dropdown-content {
    display: block;
  }
}

.login-btn {
  display: flex;
  align-items: center;
  gap: 8px;
  padding: 8px 16px;
  background: linear-gradient(135deg, #555555, #333333);
  color: white;
  border-radius: 24px;
  text-decoration: none;
  font-weight: 600;
  transition: all 0.3s ease;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
}

.login-btn:hover {
  transform: translateY(-2px);
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
  background: linear-gradient(135deg, #444444, #222222);
}

.login-btn:active {
  transform: translateY(0);
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
}

.dropdown-divider {
  margin: 8px 0;
  border-top: 1px solid rgba(85, 85, 85, 0.1);
}

.user-dropdown {
  position: relative;
  display: inline-block;
}

.user-dropdown .nav-link {
  padding: 8px 16px;
  background: linear-gradient(135deg, #555555, #333333);
  color: white;
  border-radius: 24px;
  font-weight: 600;
  transition: all 0.3s ease;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
}

.user-dropdown .nav-link::after {
  display: none;
}

.user-dropdown .nav-link:hover {
  transform: translateY(-2px);
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
  background: linear-gradient(135deg, #444444, #222222);
}

.user-dropdown .dropdown-content {
  position: absolute;
  top: 100%;
  right: 0;
  display: none;
  background: rgba(255, 255, 255, 0.98);
  min-width: 180px;
  box-shadow: 0 8px 16px rgba(0, 0, 0, 0.1);
  z-index: 1;
  border-radius: 10px;
  padding: 8px 0;
  margin-top: 10px;
  opacity: 0;
  visibility: hidden;
  transition: all 0.3s ease;
}

.user-dropdown .dropdown-content::before {
  content: '';
  position: absolute;
  top: -10px;
  left: 0;
  width: 100%;
  height: 10px;
  background: transparent;
}

.user-dropdown:hover .dropdown-content {
  display: block;
  opacity: 1;
  visibility: visible;
}

.user-dropdown .dropdown-item {
  padding: 10px 16px;
  color: #555555;
  text-decoration: none;
  display: flex;
  align-items: center;
  transition: all 0.3s ease;
  font-weight: 500;
}

.user-dropdown .dropdown-item:hover {
  background-color: rgba(85, 85, 85, 0.08);
  transform: translateX(3px);
}

.user-dropdown .dropdown-item.text-danger {
  color: #ff4d4f;
}

.user-dropdown .dropdown-item.text-danger:hover {
  background-color: rgba(255, 77, 79, 0.08);
}
</style>