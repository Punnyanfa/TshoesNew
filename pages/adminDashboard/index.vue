<template>
  <div class="container">
    <h1 class="text-primary text-center">Admin Dashboard</h1>
    <div class="row mt-4">
      <div class="col-md-4">
        <NuxtLink to="/admin/orders" class="btn btn-outline-primary w-100">Orders</NuxtLink>
      </div>
      <div class="col-md-4">
        <NuxtLink to="/admin/products" class="btn btn-outline-primary w-100">Products</NuxtLink>
      </div>
      <div class="col-md-4">
        <NuxtLink to="/admin/users" class="btn btn-outline-primary w-100">Users</NuxtLink>
      </div>
    </div>
  </div>
</template>
