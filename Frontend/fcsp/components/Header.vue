<template>
  <header class="navbar navbar-expand-lg navbar-dark bg-dark px-3 d-flex justify-content-between align-items-center">
    <nav class="navbar-nav d-flex gap-4">
      <router-link to="/homePage" class="nav-link">
        <i class="bi bi-house"></i> Home
      </router-link>
      <router-link to="/aboutPage" class="nav-link">
        <i class="bi bi-person"></i> About
      </router-link>
      <router-link to="/productPage" class="nav-link">
        <i class="bi bi-bag"></i> Products
      </router-link>
      <router-link to="/contactPage" class="nav-link">
        <i class="bi bi-envelope"></i> Contact
      </router-link>
      <router-link to="/customPage" class="nav-link">
        <i class="bi bi-ui-checks"></i> Customize
      </router-link>
    </nav>

    <div class="d-flex align-items-center gap-3">
      <!-- Ant Design Button for Search -->
      <a-button class="btn btn-outline-light" icon="search" shape="circle" />
      
      <!-- Ant Design Cart Button -->
      <router-link to="/cartPage" class="btn btn-outline-light position-relative">
        <i class="bi bi-cart"></i>
        <span class="position-absolute top-0 start-100 translate-middle badge bg-success">2</span>
      </router-link>

      <!-- Login/Register Button with Ant Design Button -->
      <router-link v-if="!isAuthenticated" to="/loginPage" class="btn btn-outline-light">
        <i class="bi bi-person"></i> Login / Register
      </router-link>
      
      <!-- Ant Design Dropdown for user info -->
      <a-dropdown v-else>
        <a-button>
          <i class="bi bi-person"></i> User <a-icon type="down" />
        </a-button>
        <a-dropdown-menu slot="overlay">
          <a-dropdown-item>Profile</a-dropdown-item>
          <a-dropdown-item>Settings</a-dropdown-item>
          <a-dropdown-item>Logout</a-dropdown-item>
        </a-dropdown-menu>
      </a-dropdown>
    </div>
  </header>
</template>

<style scoped>
.nav-link {
  font-size: 1rem;
  font-weight: 600;
  padding: 0.5rem 1rem;
  text-decoration: none;
  color: white;
  transition: all 0.3s ease-in-out;
  display: flex;
  align-items: center;
  gap: 0.5rem;
}
.nav-link:hover {
  color: #8bc34a;
  transform: translateY(-2px);
  background-color: rgba(248, 212, 157, 0.1);
  border-radius: 5px;
}

.btn {
  padding: 0.4rem 0.8rem;
  font-size: 0.9rem;
  transition: all 0.3s;
}
.btn-outline-light:hover {
  background-color: #8bc34a;
  color: #000;
  transform: translateY(-1px);
}

.badge {
  font-size: 0.7rem;
  font-weight: bold;
  border-radius: 50%;
  padding: 0.25em 0.5em;
  background-color: #28a745;
  color: white;
}

header {
  background-color: #343a40;
  border-bottom: 3px solid #8bc34a;
  padding: 1rem 2rem;
  box-shadow: 0px 4px 8px rgba(0, 0, 0, 0.15);
  transition: box-shadow 0.3s ease-in-out;
}
header:hover {
  box-shadow: 0px 6px 12px rgba(0, 0, 0, 0.2);
}
</style>
