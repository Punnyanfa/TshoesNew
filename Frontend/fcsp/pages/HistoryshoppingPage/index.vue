<template>
  <Header/>
    <div class="container py-5">
      <h1 class="mb-4">Order History</h1>
      
      <!-- Filter section -->
      <div class="card mb-4">
        <div class="card-body">
          <div class="row g-3">
            <div class="col-md-4">
              <label for="status" class="form-label">Status</label>
              <select v-model="filters.status" class="form-select" id="status">
                <option value="">All Status</option>
                <option value="completed">Completed</option>
                <option value="processing">Processing</option>
                <option value="cancelled">Cancelled</option>
              </select>
            </div>
            <div class="col-md-4">
              <label for="dateRange" class="form-label">Time Range</label>
              <select v-model="filters.dateRange" class="form-select" id="dateRange">
                <option value="all">All Time</option>
                <option value="30days">Last 30 Days</option>
                <option value="6months">Last 6 Months</option>
                <option value="1year">Last Year</option>
              </select>
            </div>
            <div class="col-md-4 d-flex align-items-end">
              <button @click="applyFilters" class="btn btn-primary w-100">Filter Orders</button>
            </div>
          </div>
        </div>
      </div>
      
      <!-- Orders list -->
      <div v-if="filteredOrders.length > 0">
        <div class="card mb-3" v-for="order in paginatedOrders" :key="order.id">
          <div class="card-header d-flex justify-content-between align-items-center">
            <div>
              <span class="fw-bold">Order #{{ order.id }}</span>
              <span class="ms-3 text-muted">{{ formatDate(order.createdAt) }}</span>
            </div>
            <div>
              <span :class="getStatusBadgeClass(order.status)">{{ getStatusText(order.status) }}</span>
            </div>
          </div>
          <div class="card-body">
            <div class="row">
              <div class="col-md-8">
                <div v-if="order.orderDetail">
                  <table class="table table-bordered table-sm mb-0">
                    <tbody>
                      <tr>
                        <th>Name</th>
                        <td>{{ order.orderDetail.customShoeDesignName }}</td>
                      </tr>
                      <tr>
                        <th>Description</th>
                        <td>{{ order.orderDetail.customShoeDesignDescription }}</td>
                      </tr>
                     
                      <tr>
                        <th>Preview Image</th>
                        <td>
                          <img v-if="order.orderDetail.firstPreviewImageUrl" :src="order.orderDetail.firstPreviewImageUrl" alt="Preview" style="max-width:80px;max-height:80px;">
                          <span v-else class="text-muted">No image</span>
                        </td>
                      </tr>
                      <tr>
                        <th>Quantity</th>
                        <td>{{ order.orderDetail.quantity }}</td>
                      </tr>
                      <tr>
                        <th>Service Price</th>
                        <td>{{ formatCurrency(order.orderDetail.servicePrice) }}</td>
                      </tr>
                      <tr>
                        <th>Size</th>
                        <td>{{ order.orderDetail.sizeValue }}</td>
                      </tr>
                      <tr>
                        <th>Template Price</th>
                        <td>{{ formatCurrency(order.orderDetail.templatePrice) }}</td>
                      </tr>
                      <tr>
                        <th>Unit Price</th>
                        <td>{{ formatCurrency(order.orderDetail.unitPrice) }}</td>
                      </tr>
                    </tbody>
                  </table>
                </div>
                <div v-else>
                  <span class="text-muted">No product details available</span>
                </div>
              </div>
              <div class="col-md-4 border-start">
                <div class="d-flex justify-content-between mb-2">
                  <span>Total Payment:</span>
                  <span>{{ formatCurrency(order.totalPrice) }}</span>
                </div>
                <div class="d-flex justify-content-between mb-2">
                  <span>Payment Method:</span>
                  <span>{{ order.paymentMethod }}</span>
                </div>
                <div class="d-flex justify-content-between mb-2">
                  <span>Shipping Status:</span>
                  <span>{{ order.shippingStatus }}</span>
                </div>
                <div class="mt-3">
                  <!-- <button class="btn btn-outline-primary btn-sm w-100 mb-2">View Details</button> -->
                  <button 
                    v-if="order.status === 'Completed' && !order.rating" 
                    @click="openRatingModal(order)" 
                    class="btn btn-outline-success btn-sm w-100 mb-2">
                    Rate Order
                  </button>
                  <div v-if="order.rating" class="text-center">
                    <div class="d-flex align-items-center justify-content-center">
                      <span class="me-2">Your Rating:</span>
                      <div class="text-warning">
                        <i v-for="n in 5" :key="n" 
                           class="bi" 
                           :class="n <= order.rating ? 'bi-star-fill' : 'bi-star'">
                        </i>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
        
        <!-- Pagination -->
        <nav aria-label="Page navigation">
          <ul class="pagination justify-content-center">
            <li class="page-item" :class="{ disabled: currentPage === 1 }">
              <a class="page-link" href="#" @click.prevent="changePage(currentPage - 1)">Previous</a>
            </li>
            <li v-for="page in totalPages" :key="page" class="page-item" :class="{ active: page === currentPage }">
              <a class="page-link" href="#" @click.prevent="changePage(page)">{{ page }}</a>
            </li>
            <li class="page-item" :class="{ disabled: currentPage === totalPages }">
              <a class="page-link" href="#" @click.prevent="changePage(currentPage + 1)">Next</a>
            </li>
          </ul>
        </nav>
      </div>
      
      <!-- Empty state -->
      <div v-else class="text-center py-5">
        <div class="mb-3">
          <i class="bi bi-bag-x" style="font-size: 3rem;"></i>
        </div>
        <h4>No Orders Found</h4>
        <p class="text-muted">You don't have any orders or no orders match the current filters.</p>
        <button class="btn btn-primary">Continue Shopping</button>
      </div>
    </div>

    <!-- Add Rating Modal -->
    <div class="modal fade" id="ratingModal" tabindex="-1" aria-labelledby="ratingModalLabel" aria-hidden="true">
      <div class="modal-dialog">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title" id="ratingModalLabel">Rate Your Order</h5>
            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
          </div>
          <div class="modal-body">
            <div class="text-center mb-3">
              <div class="rating-stars">
                <i v-for="n in 5" 
                   :key="n" 
                   class="bi bi-star-fill fs-3 me-2" 
                   :class="{ 'text-warning': n <= selectedRating }"
                   @click="selectedRating = n"
                   style="cursor: pointer;">
                </i>
              </div>
            </div>
            <div class="mb-3">
              <label for="ratingComment" class="form-label">Your Review (Optional)</label>
              <textarea 
                class="form-control" 
                id="ratingComment" 
                v-model="ratingComment" 
                rows="3"
                placeholder="Share your experience with this order...">
              </textarea>
            </div>
          </div>
          <div class="modal-footer">
            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
            <button type="button" class="btn btn-primary" @click="submitRating">Submit Rating</button>
          </div>
        </div>
      </div>
    </div>
  </template>
  
  <script setup>
  import { ref, computed, onMounted } from 'vue';
  import { getOrdersByUserId } from '@/server/order-service';
import Header from '~/components/Header.vue';
import { postRating } from '@/server/rating-service';
  
  const orders = ref([]);
  const loading = ref(true);
  const error = ref(null);
  
  // Declare userId as a reactive reference, initialized to null or undefined
  const userId = ref(null);
  
  // Declare ratingModal as a ref or let outside onMounted if needed elsewhere
  // If only used in onMounted and helper functions, a simple `let` is fine.
  let ratingModalInstance = null; // Use a different variable name to avoid conflict with the type import

  onMounted(async () => {
    // Move localStorage access inside onMounted
    if (process.client) {
      userId.value = localStorage.getItem('userId');
    }

    if (!userId.value) {
      error.value = 'Không tìm thấy userId.';
      loading.value = false;
      return;
    }

    // Import Bootstrap JS and initialize modal on the client side
    if (process.client) {
      try {
        const bootstrap = await import('bootstrap/dist/js/bootstrap.bundle.min.js');
        // Initialize Bootstrap modal
        ratingModalInstance = new bootstrap.Modal(document.getElementById('ratingModal'));
      } catch (e) {
        console.error('Error loading Bootstrap or initializing modal:', e);
      }
    }

    try {
      // Use userId.value to access the ref's value
      const res = await getOrdersByUserId(userId.value);
      console.log('res', res);
      orders.value = res.orders || [];
    } catch (err) {
      error.value = err.message || 'Lỗi khi tải đơn hàng.';
    } finally {
      loading.value = false;
    }
  });
  
  // Filters
  const filters = ref({
    status: '',
    dateRange: 'all'
  });
  
  // Pagination
  const currentPage = ref(1);
  const itemsPerPage = 3;
  
  // Apply filters
  const applyFilters = () => {
    currentPage.value = 1;
  };
  
  // Filter orders based on selected filters
  const filteredOrders = computed(() => {
    let result = [...orders.value];
    
    // Filter by status
    if (filters.value.status) {
      result = result.filter(order => order.status === filters.value.status);
    }
    
    // Filter by date range
    if (filters.value.dateRange !== 'all') {
      const now = new Date();
      let startDate;
      
      switch (filters.value.dateRange) {
        case '30days':
          startDate = new Date(now.setDate(now.getDate() - 30));
          break;
        case '6months':
          startDate = new Date(now.setMonth(now.getMonth() - 6));
          break;
        case '1year':
          startDate = new Date(now.setFullYear(now.getFullYear() - 1));
          break;
        default:
          startDate = null;
      }
      
      if (startDate) {
        result = result.filter(order => new Date(order.createdAt) >= startDate);
      }
    }
    
    return result;
  });
  
  // Calculate total pages
  const totalPages = computed(() => {
    return Math.ceil(filteredOrders.value.length / itemsPerPage);
  });
  
  // Get paginated orders
  const paginatedOrders = computed(() => {
    const startIndex = (currentPage.value - 1) * itemsPerPage;
    const endIndex = startIndex + itemsPerPage;
    return filteredOrders.value.slice(startIndex, endIndex);
  });
  
  // Change page
  const changePage = (page) => {
    if (page >= 1 && page <= totalPages.value) {
      currentPage.value = page;
    }
  };
  
  // Format date
  const formatDate = (dateString) => {
    const date = new Date(dateString);
    return date.toLocaleDateString('en-US', {
      day: '2-digit',
      month: '2-digit',
      year: 'numeric',
      hour: '2-digit',
      minute: '2-digit'
    });
  };
  
  // Format currency
  const formatCurrency = (amount) => {
    return new Intl.NumberFormat('en-US', {
      style: 'currency',
      currency: 'USD'
    }).format(amount);
  };
  
  // Get status text
  const getStatusText = (status) => {
    switch (status) {
      case 'completed':
      case 'Processing':
        return 'Processing';
      case 'cancelled':
      case 'Cancelled':
        return 'Cancelled';
      case 'confirmed':
      case 'Completed':
        return 'Completed';
      default:
        return status;
    }
  };
  
  // Get status badge class
  const getStatusBadgeClass = (status) => {
    let baseClass = 'badge ';
    
    switch (status) {
      case 'completed':
      case 'Completed':
        return baseClass + 'bg-success';
      case 'processing':
      case 'Processing':
        return baseClass + 'bg-primary';
      case 'cancelled':
      case 'Cancelled':
        return baseClass + 'bg-danger';
      default:
        return baseClass + 'bg-secondary';
    }
  };
  
  // Rating related
  const selectedOrder = ref(null);
  const selectedRating = ref(0);
  const ratingComment = ref('');

  const openRatingModal = (order) => {
    selectedOrder.value = order;
    selectedRating.value = 0;
    ratingComment.value = '';
    // Use the initialized modal instance
    if (ratingModalInstance) {
      ratingModalInstance.show();
    } else {
      console.error('Rating modal not initialized.');
      // Potentially handle this case, e.g., show an error message to the user
    }
  };

  const submitRating = async () => {
    if (!selectedRating.value) {
      alert('Please select a rating');
      return;
    }

    const customShoeDesignId = selectedOrder.value.orderDetail?.customShoeDesignId;
    if (!customShoeDesignId) {
        alert('Could not find custom shoe design ID for this order.');
        return;
    }

    // Ensure userId has a value before proceeding (it should be set in onMounted)
    if (!userId.value) {
        alert('User ID not available. Please log in again.');
        return;
    }

    const ratingData = {
      userId: Number(userId.value),  // Use userId.value here
      customShoeDesignId: customShoeDesignId,
      value: selectedRating.value,
      comment: ratingComment.value
    };

    try {
      // Make the API call to save the rating
      const response = await postRating(ratingData);
      console.log('Rating submitted successfully:', response);

      // Update the local state for the specific order with the new rating
      const orderIndex = orders.value.findIndex(o => o.id === selectedOrder.value.id);
      if (orderIndex !== -1) {
        // Assuming the API response might return the updated order or just a success status.
        // For now, let's manually update the local order object with the rating.
        orders.value[orderIndex] = {
          ...orders.value[orderIndex],
          rating: selectedRating.value, // Store the rating value
          ratingComment: ratingComment.value // Store the comment as well
        };
        // Trigger reactivity by creating a new array reference or updating in place carefully
        // This might be necessary depending on Vue version and how reactivity is handled
        orders.value = [...orders.value];
      }
      
      // Use the initialized modal instance to hide
      if (ratingModalInstance) {
        ratingModalInstance.hide();
      } else {
         console.error('Rating modal not initialized.');
      }

      // Reset form
      selectedOrder.value = null;
      selectedRating.value = 0;
      ratingComment.value = '';

      alert('Rating submitted successfully!');

    } catch (error) {
      console.error('Error submitting rating:', error);
      alert('Failed to submit rating. Please try again.');
    }
  };
  </script>
  
  <style scoped>
  /* Add Bootstrap icons */
  @import url("https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.0/font/bootstrap-icons.css");
  
  /* Add Bootstrap CSS */
  @import url("https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css");
  
  .card {
    border-radius: 8px;
    box-shadow: 0 2px 4px rgba(0, 0, 0, 0.05);
  }
  
  .card-header {
    background-color: #f8f9fa;
    border-bottom: 1px solid #eaeaea;
  }
  
  .img-thumbnail {
    border-radius: 6px;
  }
  
  .pagination .page-link {
    color: #777777;
  }
  
  .pagination .page-item.active .page-link {
    background-color: #777777;
    border-color: #777777;
  }

  .rating-stars .bi-star-fill {
    color: #dee2e6;
    transition: color 0.2s ease-in-out;
  }

  .rating-stars .bi-star-fill.text-warning {
    color: #ffc107;
  }

  .rating-stars .bi-star-fill:hover {
    color: #ffc107;
    cursor: pointer;
  }
  </style>