<template>
    <div class="container-fluid py-4">
      <div class="row mb-4">
        <div class="col-12">
          <h1 class="mb-3 text-primary fw-bold d-flex align-items-center">
            <i class="bi bi-chat-square-text me-2"></i>
            Quản lý đánh giá & bình luận
          </h1>
        </div>
      </div>
      
      <!-- Stats Cards -->
      <div class="row g-3 mb-4">
        <div class="col-sm-6 col-xl-3">
          <div class="stat-card bg-gradient">
            <div class="stat-icon bg-primary text-white">
              <i class="bi bi-chat-square-text"></i>
            </div>
            <div class="stat-content">
              <h6 class="stat-label">Tổng đánh giá</h6>
              <h2 class="stat-value">{{ comments.length }}</h2>
            </div>
          </div>
        </div>
        <div class="col-sm-6 col-xl-3">
          <div class="stat-card bg-gradient">
            <div class="stat-icon bg-success text-white">
              <i class="bi bi-star"></i>
            </div>
            <div class="stat-content">
              <h6 class="stat-label">Đánh giá trung bình</h6>
              <h2 class="stat-value">4.2/5</h2>
            </div>
          </div>
        </div>
        <div class="col-sm-6 col-xl-3">
          <div class="stat-card bg-gradient">
            <div class="stat-icon bg-warning text-dark">
              <i class="bi bi-hourglass-split"></i>
            </div>
            <div class="stat-content">
              <h6 class="stat-label">Chờ duyệt</h6>
              <h2 class="stat-value">{{ pendingCount }}</h2>
            </div>
          </div>
        </div>
        <div class="col-sm-6 col-xl-3">
          <div class="stat-card bg-gradient">
            <div class="stat-icon bg-info text-white">
              <i class="bi bi-reply"></i>
            </div>
            <div class="stat-content">
              <h6 class="stat-label">Tỷ lệ phản hồi</h6>
              <h2 class="stat-value">75%</h2>
            </div>
          </div>
        </div>
      </div>
      
      <!-- Search and Filter -->
      <div class="filter-container mb-4">
        <div class="row g-3">
          <div class="col-md-4">
            <div class="search-box">
              <i class="bi bi-search"></i>
              <input type="text" class="form-control" v-model="search" placeholder="Tìm kiếm đánh giá...">
              <button v-if="search" class="btn-clear" @click="search = ''">
                <i class="bi bi-x"></i>
              </button>
            </div>
          </div>
          <div class="col-md-8">
            <div class="filter-pills">
              <select class="filter-select" v-model="productFilter">
                <option value="">Tất cả sản phẩm</option>
                <option v-for="product in uniqueProducts" :key="product" :value="product">{{ product }}</option>
              </select>
              
              <select class="filter-select" v-model="ratingFilter">
                <option value="">Tất cả sao</option>
                <option v-for="i in 5" :key="i" :value="i">{{ i }} sao</option>
              </select>
              
              <select class="filter-select" v-model="statusFilter">
                <option value="">Tất cả trạng thái</option>
                <option value="approved">Đã duyệt</option>
                <option value="pending">Chờ duyệt</option>
                <option value="rejected">Đã từ chối</option>
              </select>
            </div>
          </div>
        </div>
      </div>
  
      <!-- Comments Table -->
      <div class="comments-container">
        <div v-if="loading" class="text-center py-5">
          <div class="spinner-border text-primary" role="status">
            <span class="visually-hidden">Đang tải...</span>
          </div>
        </div>
        
        <div v-else-if="filteredComments.length === 0" class="empty-state">
          <i class="bi bi-inbox-fill"></i>
          <p>Không tìm thấy đánh giá nào</p>
        </div>
        
        <div v-else class="comment-list">
          <div v-for="comment in filteredComments" :key="comment.id" class="comment-card">
            <div class="comment-header">
              <div class="product-info">
                <img :src="comment.productImage" class="product-img" alt="Product">
                <div>
                  <div class="product-name">{{ comment.productName }}</div>
                  <div class="product-sku">SKU: {{ comment.productSku }}</div>
                </div>
              </div>
              <div class="comment-status">
                <span :class="['status-badge', getStatusClass(comment.status)]">
                  {{ getStatusText(comment.status) }}
                </span>
              </div>
            </div>
            
            <div class="comment-body">
              <div class="user-info">
                <img :src="comment.userAvatar || 'https://ui-avatars.com/api/?name=' + encodeURIComponent(comment.userName) + '&background=random'" 
                     class="user-avatar" alt="User">
                <div>
                  <div class="user-name">{{ comment.userName }}</div>
                  <div class="rating-stars">
                    <i v-for="i in 5" :key="i" class="bi" 
                       :class="i <= comment.rating ? 'bi-star-fill' : 'bi-star'"></i>
                  </div>
                </div>
                <div class="comment-date">
                  <i class="bi bi-calendar3"></i> {{ formatDate(comment.date) }}
                </div>
              </div>
              
              <div class="comment-content">
                <p>{{ comment.comment }}</p>
                <div v-if="comment.response" class="comment-response">
                  <div class="response-header">
                    <i class="bi bi-reply-fill"></i> Phản hồi
                  </div>
                  <p>{{ comment.response }}</p>
                </div>
              </div>
            </div>
            
            <div class="comment-actions">
              <button class="action-btn view-btn" @click="handleComment(comment, 'view')">
                <i class="bi bi-eye"></i>
              </button>
              <button v-if="comment.status === 'pending'" class="action-btn approve-btn" 
                      @click="handleComment(comment, 'approve')">
                <i class="bi bi-check-lg"></i>
              </button>
              <button v-if="comment.status === 'pending'" class="action-btn reject-btn" 
                      @click="handleComment(comment, 'reject')">
                <i class="bi bi-x-lg"></i>
              </button>
              <button v-if="comment.status !== 'pending'" class="action-btn respond-btn" 
                      @click="handleComment(comment, 'respond')">
                <i class="bi bi-reply"></i>
              </button>
            </div>
          </div>
        </div>
        
        <div class="comments-footer">
          <div>Hiển thị {{ filteredComments.length }} / {{ comments.length }} đánh giá</div>
          <button v-if="pendingCount > 0" class="bulk-action-btn" @click="showBulkModal">
            <i class="bi bi-check2-all me-1"></i> Duyệt hàng loạt ({{ pendingCount }})
          </button>
        </div>
      </div>
  
      <!-- Universal Modal -->
      <div class="modal fade" id="commentModal" tabindex="-1" ref="commentModal">
        <div class="modal-dialog modal-dialog-centered" :class="{'modal-lg': modalType === 'view' || modalType === 'bulk'}">
          <div class="modal-content">
            <div class="modal-header" :class="getModalHeaderClass()">
              <h5 class="modal-title">{{ getModalTitle() }}</h5>
              <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"></button>
            </div>
            
            <!-- View Comment Details -->
            <div v-if="modalType === 'view'" class="modal-body">
              <div class="view-layout" v-if="selectedComment">
                <div class="product-section">
                  <div class="product-card">
                    <img :src="selectedComment.productImage" class="product-image">
                    <h6 class="product-title">{{ selectedComment.productName }}</h6>
                    <p class="product-code">SKU: {{ selectedComment.productSku }}</p>
                  </div>
                </div>
                
                <div class="comment-section">
                  <div class="comment-card-detail">
                    <div class="user-header">
                      <img :src="selectedComment.userAvatar || 'https://ui-avatars.com/api/?name=' + encodeURIComponent(selectedComment.userName) + '&background=random'" 
                           class="user-pic">
                      <div>
                        <div class="user-title">{{ selectedComment.userName }}</div>
                        <div class="user-email">{{ selectedComment.userEmail }}</div>
                      </div>
                      <div class="status-tag" :class="getStatusClass(selectedComment.status)">
                        {{ getStatusText(selectedComment.status) }}
                      </div>
                    </div>
                    
                    <div class="rating-display">
                      <i v-for="i in 5" :key="i" class="bi" 
                         :class="i <= selectedComment.rating ? 'bi-star-fill' : 'bi-star'"></i>
                      <span>{{ selectedComment.rating }}/5</span>
                    </div>
                    
                    <div class="comment-text">
                      <p>{{ selectedComment.comment }}</p>
                    </div>
                    
                    <div v-if="selectedComment.response" class="response-box">
                      <h6><i class="bi bi-reply-fill"></i> Phản hồi</h6>
                      <p>{{ selectedComment.response }}</p>
                    </div>
                  </div>
                </div>
              </div>
            </div>
            
            <!-- Response Form -->
            <div v-if="modalType === 'respond'" class="modal-body">
              <div class="original-comment" v-if="selectedComment">
                <p>{{ selectedComment.comment }}</p>
              </div>
              <div class="response-form">
                <label for="responseText">Phản hồi của bạn</label>
                <textarea id="responseText" rows="4" v-model="responseText" 
                          placeholder="Nhập phản hồi của bạn..."></textarea>
              </div>
            </div>
            
            <!-- Approve/Reject Confirmation -->
            <div v-if="modalType === 'approve' || modalType === 'reject'" class="modal-body">
              <div class="confirmation-message">
                <p>Bạn có chắc chắn muốn {{ modalType === 'approve' ? 'duyệt' : 'từ chối' }} đánh giá này?</p>
              </div>
              <div class="comment-preview" v-if="selectedComment">
                <p>{{ selectedComment.comment }}</p>
              </div>
              <div v-if="modalType === 'reject'" class="reject-reason">
                <label for="rejectReason">Lý do từ chối (tùy chọn)</label>
                <textarea id="rejectReason" rows="3" v-model="rejectReason" 
                          placeholder="Nhập lý do từ chối..."></textarea>
              </div>
            </div>
            
            <!-- Bulk Action -->
            <div v-if="modalType === 'bulk'" class="modal-body">
              <div class="bulk-header">
                <div class="alert-box">
                  <i class="bi bi-info-circle"></i>
                  Có <strong>{{ pendingCount }}</strong> đánh giá đang chờ duyệt.
                </div>
                
                <div class="select-all-option">
                  <input type="checkbox" id="selectAll" v-model="selectAll" @change="toggleSelectAll">
                  <label for="selectAll">Chọn tất cả</label>
                </div>
              </div>
              
              <div class="bulk-comments">
                <div v-for="comment in pendingComments" :key="comment.id" class="bulk-comment-item">
                  <div class="selection">
                    <input type="checkbox" :id="'comment-' + comment.id" v-model="selectedIds" :value="comment.id">
                  </div>
                  <div class="comment-info">
                    <div class="product-name">{{ comment.productName }}</div>
                    <div class="user-name">{{ comment.userName }}</div>
                    <div class="rating-stars">
                      <i v-for="i in 5" :key="i" class="bi" 
                         :class="i <= comment.rating ? 'bi-star-fill' : 'bi-star'"></i>
                    </div>
                    <div class="comment-preview">{{ comment.comment }}</div>
                  </div>
                </div>
              </div>
            </div>
            
            <!-- Modal Footer -->
            <div class="modal-footer">
              <button type="button" class="cancel-btn" data-bs-dismiss="modal">
                {{ modalType === 'view' ? 'Đóng' : 'Hủy' }}
              </button>
              
              <template v-if="modalType === 'view' && selectedComment">
                <button v-if="selectedComment.status === 'pending'" class="approve-btn-lg" 
                        @click="handleComment(selectedComment, 'approve')">
                  <i class="bi bi-check-lg"></i> Duyệt
                </button>
                <button v-if="selectedComment.status === 'pending'" class="reject-btn-lg" 
                        @click="handleComment(selectedComment, 'reject')">
                  <i class="bi bi-x-lg"></i> Từ chối
                </button>
                <button v-if="selectedComment.status !== 'pending'" class="respond-btn-lg" 
                        @click="handleComment(selectedComment, 'respond')">
                  <i class="bi bi-reply"></i> Phản hồi
                </button>
              </template>
              
              <button v-if="modalType === 'respond'" class="submit-btn" @click="submitAction">
                Gửi phản hồi
              </button>
              
              <button v-if="modalType === 'approve' || modalType === 'reject'" 
                      :class="modalType === 'approve' ? 'approve-btn-lg' : 'reject-btn-lg'"
                      @click="submitAction">
                {{ modalType === 'approve' ? 'Duyệt' : 'Từ chối' }}
              </button>
              
              <template v-if="modalType === 'bulk'">
                <button class="reject-btn-lg" @click="bulkAction('reject')" 
                        :disabled="selectedIds.length === 0">
                  <i class="bi bi-x-lg"></i> Từ chối ({{ selectedIds.length }})
                </button>
                <button class="approve-btn-lg" @click="bulkAction('approve')" 
                        :disabled="selectedIds.length === 0">
                  <i class="bi bi-check-lg"></i> Duyệt ({{ selectedIds.length }})
                </button>
              </template>
            </div>
          </div>
        </div>
      </div>
    </div>
  </template>
  
  <script>
  export default {
    name: 'CommentsAndRatingsManagement',
    data() {
      return {
        search: '',
        productFilter: '',
        ratingFilter: '',
        statusFilter: '',
        loading: false,
        selectedComment: null,
        responseText: '',
        rejectReason: '',
        selectAll: false,
        selectedIds: [],
        modalType: 'view',
        commentModal: null,
        comments: [
          {
            id: 'CMT001',
            productName: 'Nike Air Max 270',
            productSku: 'NKE-AM270-BLK',
            productImage: 'https://via.placeholder.com/100x100.png?text=Nike+Air+Max+270',
            userName: 'Nguyễn Văn A',
            userEmail: 'nguyenvana@example.com',
            userAvatar: 'https://ui-avatars.com/api/?name=Nguyen+Van+A&background=0D8ABC&color=fff',
            rating: 5,
            comment: 'Giày rất đẹp và thoải mái, đúng như mô tả. Tôi rất hài lòng với sản phẩm này!',
            status: 'approved',
            date: '2024-03-25',
            response: 'Cảm ơn bạn đã đánh giá tích cực về sản phẩm của chúng tôi!'
          },
          {
            id: 'CMT002',
            productName: 'Adidas Ultra Boost',
            productSku: 'ADS-UB21-WHT',
            productImage: 'https://via.placeholder.com/100x100.png?text=Adidas+Ultra+Boost',
            userName: 'Trần Thị B',
            userEmail: 'tranthib@example.com',
            userAvatar: null,
            rating: 4,
            comment: 'Giày chạy bộ rất tốt, đệm êm ái. Tuy nhiên size hơi rộng một chút so với chân tôi.',
            status: 'approved',
            date: '2024-03-24',
            response: 'Cảm ơn bạn đã đánh giá. Về vấn đề size, bạn có thể liên hệ với chúng tôi để được tư vấn.'
          },
          {
            id: 'CMT003',
            productName: 'Puma RS-X',
            productSku: 'PMA-RSX-BLU',
            productImage: 'https://via.placeholder.com/100x100.png?text=Puma+RS-X',
            userName: 'Lê Văn C',
            userEmail: 'levanc@example.com',
            userAvatar: null,
            rating: 2,
            comment: 'Giày không đúng như mô tả, màu sắc khác xa so với hình ảnh trên website. Rất thất vọng!',
            status: 'approved',
            date: '2024-03-23',
            response: 'Chúng tôi rất tiếc về trải nghiệm không tốt của bạn. Vui lòng liên hệ với bộ phận CSKH.'
          },
          {
            id: 'CMT004',
            productName: 'Jordan 1 Retro High',
            productSku: 'JRD-1RH-RED',
            productImage: 'https://via.placeholder.com/100x100.png?text=Jordan+1+Retro+High',
            userName: 'Phạm Thị D',
            userEmail: 'phamthid@example.com',
            userAvatar: null,
            rating: 5,
            comment: 'Giày đẹp tuyệt vời, đúng chất Jordan. Giao hàng nhanh, đóng gói cẩn thận!',
            status: 'pending',
            date: '2024-03-28',
            response: null
          },
          {
            id: 'CMT005',
            productName: 'Converse Chuck 70',
            productSku: 'CNV-C70-BLK',
            productImage: 'https://via.placeholder.com/100x100.png?text=Converse+Chuck+70',
            userName: 'Hoàng Văn E',
            userEmail: 'hoangvane@example.com',
            userAvatar: null,
            rating: 4,
            comment: 'Giày Converse Chuck 70 chất lượng tốt hơn bản thường rất nhiều. Đáng đồng tiền bát gạo!',
            status: 'pending',
            date: '2024-03-27',
            response: null
          }
        ]
      }
    },
    computed: {
      filteredComments() {
        let result = [...this.comments];
        
        if (this.productFilter) {
          result = result.filter(c => c.productName === this.productFilter);
        }
        
        if (this.ratingFilter) {
          result = result.filter(c => c.rating === parseInt(this.ratingFilter));
        }
        
        if (this.statusFilter) {
          result = result.filter(c => c.status === this.statusFilter);
        }
        
        if (this.search) {
          const searchLower = this.search.toLowerCase();
          result = result.filter(c => 
            c.productName.toLowerCase().includes(searchLower) ||
            c.userName.toLowerCase().includes(searchLower) ||
            c.comment.toLowerCase().includes(searchLower)
          );
        }
        
        return result;
      },
      uniqueProducts() {
        return [...new Set(this.comments.map(c => c.productName))];
      },
      pendingComments() {
        return this.comments.filter(c => c.status === 'pending');
      },
      pendingCount() {
        return this.pendingComments.length;
      }
    },
    methods: {
      formatDate(date) {
        return new Date(date).toLocaleDateString('vi-VN');
      },
      getStatusText(status) {
        return {
          'approved': 'Đã duyệt',
          'pending': 'Chờ duyệt',
          'rejected': 'Đã từ chối'
        }[status] || status;
      },
      getStatusClass(status) {
        return {
          'approved': 'status-approved',
          'pending': 'status-pending',
          'rejected': 'status-rejected'
        }[status] || '';
      },
      getModalHeaderClass() {
        return {
          'view': 'header-primary',
          'respond': 'header-info',
          'approve': 'header-success',
          'reject': 'header-danger',
          'bulk': 'header-primary'
        }[this.modalType] || 'header-primary';
      },
      getModalTitle() {
        return {
          'view': 'Chi tiết đánh giá',
          'respond': 'Phản hồi đánh giá',
          'approve': 'Duyệt đánh giá',
          'reject': 'Từ chối đánh giá',
          'bulk': 'Duyệt đánh giá hàng loạt'
        }[this.modalType] || 'Quản lý đánh giá';
      },
      handleComment(comment, action) {
        this.selectedComment = comment;
        this.modalType = action;
        
        if (action === 'respond') {
          this.responseText = comment.response || '';
        }
        
        this.commentModal.show();
      },
      showBulkModal() {
        this.modalType = 'bulk';
        this.selectedIds = [];
        this.selectAll = false;
        this.commentModal.show();
      },
      submitAction() {
        if (this.modalType === 'approve') {
          this.updateStatus('approved');
        } else if (this.modalType === 'reject') {
          this.updateStatus('rejected');
        } else if (this.modalType === 'respond') {
          this.submitResponse();
        }
      },
      updateStatus(status) {
        if (this.selectedComment) {
          this.selectedComment.status = status;
          this.showToast(`Đánh giá đã được ${status === 'approved' ? 'duyệt' : 'từ chối'} thành công`);
          this.commentModal.hide();
        }
      },
      submitResponse() {
        if (this.selectedComment && this.responseText.trim()) {
          this.selectedComment.response = this.responseText;
          this.showToast('Phản hồi đã được gửi thành công');
          this.commentModal.hide();
        }
      },
      toggleSelectAll() {
        this.selectedIds = this.selectAll 
          ? this.pendingComments.map(c => c.id) 
          : [];
      },
      bulkAction(action) {
        if (this.selectedIds.length === 0) return;
        
        const status = action === 'approve' ? 'approved' : 'rejected';
        
        this.comments.forEach(comment => {
          if (this.selectedIds.includes(comment.id)) {
            comment.status = status;
          }
        });
        
        this.showToast(`Đã ${action === 'approve' ? 'duyệt' : 'từ chối'} ${this.selectedIds.length} đánh giá thành công`);
        this.commentModal.hide();
      },
      showToast(message, type = 'success') {
        const toastContainer = document.getElementById('toast-container') || this.createToastContainer();
        
        const toastEl = document.createElement('div');
        toastEl.className = `toast align-items-center text-white bg-${type} border-0`;
        toastEl.setAttribute('role', 'alert');
        toastEl.setAttribute('aria-live', 'assertive');
        
        toastEl.innerHTML = `
          <div class="d-flex">
            <div class="toast-body">${message}</div>
            <button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast"></button>
          </div>
        `;
        
        toastContainer.appendChild(toastEl);
        // const toast = new bootstrap.Toast(toastEl, { autohide: true, delay: 3000 });
        toast.show();
      },
      createToastContainer() {
        const container = document.createElement('div');
        container.id = 'toast-container';
        container.className = 'toast-container position-fixed bottom-0 end-0 p-3';
        container.style.zIndex = '1050';
        document.body.appendChild(container);
        return container;
      },
      async fetchComments() {
        this.loading = true;
        try {
          await new Promise(resolve => setTimeout(resolve, 500));
        } finally {
          this.loading = false;
        }
      }
    },
    mounted() {
    //   this.commentModal = new bootstrap.Modal(this.$refs.commentModal);
      this.fetchComments();
    }
  }
  </script>
  
  <style scoped>
  /* Base Styles */
  :root {
    --primary: #4361ee;
    --success: #2ecc71;
    --warning: #f39c12;
    --danger: #e74c3c;
    --info: #3498db;
    --light: #f8f9fa;
    --dark: #343a40;
    --gray: #6c757d;
    --border-radius: 10px;
    --shadow: 0 4px 6px rgba(0, 0, 0, 0.05);
    --transition: all 0.3s ease;
  }
  
  /* Stats Cards */
  .stat-card {
    display: flex;
    align-items: center;
    padding: 1.25rem;
    border-radius: var(--border-radius);
    box-shadow: var(--shadow);
    background: linear-gradient(145deg, #ffffff, #f5f7fa);
    transition: var(--transition);
    border: none;
    height: 100%;
  }
  
  .stat-card:hover {
    transform: translateY(-5px);
    box-shadow: 0 8px 15px rgba(0, 0, 0, 0.1);
  }
  
  .stat-icon {
    width: 50px;
    height: 50px;
    display: flex;
    align-items: center;
    justify-content: center;
    border-radius: 12px;
    font-size: 1.5rem;
    margin-right: 1rem;
  }
  
  .stat-content {
    flex: 1;
  }
  
  .stat-label {
    color: var(--gray);
    margin-bottom: 0.25rem;
    font-size: 0.875rem;
    font-weight: 500;
  }
  
  .stat-value {
    font-size: 1.75rem;
    font-weight: 700;
    margin: 0;
    line-height: 1.2;
  }
  
  /* Search and Filter */
  .filter-container {
    background: white;
    border-radius: var(--border-radius);
    padding: 1.25rem;
    box-shadow: var(--shadow);
  }
  
  .search-box {
    position: relative;
    display: flex;
    align-items: center;
  }
  
  .search-box i {
    position: absolute;
    left: 12px;
    color: var(--gray);
  }
  
  .search-box input {
    padding-left: 35px;
    border-radius: 30px;
    border: 1px solid #e0e0e0;
    height: 45px;
    transition: var(--transition);
  }
  
  .search-box input:focus {
    box-shadow: 0 0 0 3px rgba(67, 97, 238, 0.15);
    border-color: var(--primary);
  }
  
  .btn-clear {
    position: absolute;
    right: 12px;
    background: none;
    border: none;
    color: var(--gray);
    cursor: pointer;
  }
  
  .filter-pills {
    display: flex;
    flex-wrap: wrap;
    gap: 10px;
  }
  
  .filter-select {
    background-color: white;
    border: 1px solid #e0e0e0;
    border-radius: 30px;
    padding: 0.5rem 1rem;
    font-size: 0.875rem;
    color: var(--dark);
    transition: var(--transition);
    cursor: pointer;
    flex-grow: 1;
  }
  
  .filter-select:focus {
    outline: none;
    box-shadow: 0 0 0 3px rgba(67, 97, 238, 0.15);
    border-color: var(--primary);
  }
  
  /* Comments Container */
  .comments-container {
    background: white;
    border-radius: var(--border-radius);
    box-shadow: var(--shadow);
    overflow: hidden;
  }
  
  .empty-state {
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
    padding: 3rem 1rem;
    color: var(--gray);
  }
  
  .empty-state i {
    font-size: 3rem;
    margin-bottom: 1rem;
    opacity: 0.5;
  }
  
  .comment-list {
    display: flex;
    flex-direction: column;
    gap: 1px;
    background-color: #f0f2f5;
  }
  
  .comment-card {
    background: white;
    padding: 1.25rem;
    transition: var(--transition);
    display: flex;
    flex-direction: column;
    gap: 1rem;
  }
  
  .comment-card:hover {
    background-color: #f8f9fa;
  }
  
  .comment-header {
    display: flex;
    justify-content: space-between;
    align-items: flex-start;
  }
  
  .product-info {
    display: flex;
    align-items: center;
    gap: 0.75rem;
  }
  
  .product-img {
    width: 40px;
    height: 40px;
    object-fit: cover;
    border-radius: 8px;
  }
  
  .product-name {
    font-weight: 600;
    margin-bottom: 0.25rem;
  }
  
  .product-sku {
    font-size: 0.75rem;
    color: var(--gray);
  }
  
  .comment-status {
    display: flex;
    align-items: center;
  }
  
  .status-badge {
    padding: 0.35em 0.65em;
    font-size: 0.75rem;
    font-weight: 600;
    border-radius: 30px;
    text-transform: uppercase;
    letter-spacing: 0.5px;
  }
  
  .status-approved {
    background-color: rgba(46, 204, 113, 0.15);
    color: var(--success);
  }
  
  .status-pending {
    background-color: rgba(243, 156, 18, 0.15);
    color: var(--warning);
  }
  
  .status-rejected {
    background-color: rgba(231, 76, 60, 0.15);
    color: var(--danger);
  }
  
  .comment-body {
    display: flex;
    flex-direction: column;
    gap: 0.75rem;
  }
  
  .user-info {
    display: flex;
    align-items: center;
    gap: 0.75rem;
  }
  
  .user-avatar {
    width: 36px;
    height: 36px;
    border-radius: 50%;
    object-fit: cover;
  }
  
  .user-name {
    font-weight: 500;
    margin-bottom: 0.25rem;
  }
  
  .rating-stars {
    display: flex;
    align-items: center;
    color: #f39c12;
    font-size: 0.875rem;
  }
  
  .comment-date {
    margin-left: auto;
    font-size: 0.75rem;
    color: var(--gray);
    display: flex;
    align-items: center;
    gap: 0.25rem;
  }
  
  .comment-content {
    background-color: #f8f9fa;
    padding: 1rem;
    border-radius: 8px;
  }
  
  .comment-content p {
    margin: 0;
    line-height: 1.5;
  }
  
  .comment-response {
    margin-top: 0.75rem;
    padding-top: 0.75rem;
    border-top: 1px dashed #e0e0e0;
  }
  
  .response-header {
    font-weight: 600;
    color: var(--primary);
    margin-bottom: 0.5rem;
    display: flex;
    align-items: center;
    gap: 0.5rem;
    font-size: 0.875rem;
  }
  
  .comment-actions {
    display: flex;
    gap: 0.5rem;
    justify-content: flex-end;
  }
  
  .action-btn {
    width: 36px;
    height: 36px;
    border-radius: 50%;
    display: flex;
    align-items: center;
    justify-content: center;
    border: none;
    cursor: pointer;
    transition: var(--transition);
    background-color: #f8f9fa;
  }
  
  .view-btn {
    color: var(--primary);
  }
  
  .approve-btn {
    color: var(--success);
  }
  
  .reject-btn {
    color: var(--danger);
  }
  
  .respond-btn {
    color: var(--info);
  }
  
  .action-btn:hover {
    transform: translateY(-2px);
  }
  
  .view-btn:hover {
    background-color: rgba(67, 97, 238, 0.1);
  }
  
  .approve-btn:hover {
    background-color: rgba(46, 204, 113, 0.1);
  }
  
  .reject-btn:hover {
    background-color: rgba(231, 76, 60, 0.1);
  }
  
  .respond-btn:hover {
    background-color: rgba(52, 152, 219, 0.1);
  }
  
  .comments-footer {
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding: 1rem 1.25rem;
    border-top: 1px solid #e0e0e0;
    color: var(--gray);
    font-size: 0.875rem;
  }
  
  .bulk-action-btn {
    background-color: var(--primary);
    color: white;
    border: none;
    border-radius: 30px;
    padding: 0.5rem 1rem;
    font-size: 0.875rem;
    font-weight: 500;
    cursor: pointer;
    transition: var(--transition);
  }
  
  .bulk-action-btn:hover {
    background-color: #3651d1;
    transform: translateY(-2px);
  }
  
  /* Modal Styling */
  .modal-content {
    border: none;
    border-radius: 12px;
    overflow: hidden;
  }
  
  .modal-header {
    border-bottom: none;
    padding: 1.25rem;
  }
  
  .header-primary {
    background-color: var(--primary);
    color: white;
  }
  
  .header-success {
    background-color: var(--success);
    color: white;
  }
  
  .header-danger {
    background-color: var(--danger);
    color: white;
  }
  
  .header-info {
    background-color: var(--info);
    color: white;
  }
  
  .modal-body {
    padding: 1.5rem;
  }
  
  .modal-footer {
    border-top: none;
    padding: 1.25rem;
  }
  
  .view-layout {
    display: grid;
    grid-template-columns: 1fr 2fr;
    gap: 1.5rem;
  }
  
  .product-card {
    background-color: #f8f9fa;
    border-radius: 12px;
    padding: 1.25rem;
    text-align: center;
  }
  
  .product-image {
    max-width: 100%;
    height: auto;
    border-radius: 8px;
    margin-bottom: 1rem;
  }
  
  .product-title {
    font-weight: 600;
    margin-bottom: 0.5rem;
  }
  
  .product-code {
    font-size: 0.75rem;
    color: var(--gray);
    margin: 0;
  }
  
  .comment-card-detail {
    background-color: #f8f9fa;
    border-radius: 12px;
    padding: 1.25rem;
  }
  
  .user-header {
    display: flex;
    align-items: center;
    gap: 0.75rem;
    margin-bottom: 1rem;
  }
  
  .user-pic {
    width: 40px;
    height: 40px;
    border-radius: 50%;
  }
  
  .user-title {
    font-weight: 600;
    margin-bottom: 0.25rem;
  }
  
  .user-email {
    font-size: 0.75rem;
    color: var(--gray);
  }
  
  .status-tag {
    margin-left: auto;
    padding: 0.35em 0.65em;
    font-size: 0.75rem;
    font-weight: 600;
    border-radius: 30px;
    text-transform: uppercase;
  }
  
  .rating-display {
    margin-bottom: 1rem;
    display: flex;
    align-items: center;
    gap: 0.5rem;
    color: #f39c12;
  }
  
  .rating-display span {
    margin-left: 0.5rem;
    font-weight: 600;
  }
  
  .comment-text {
    background-color: white;
    padding: 1rem;
    border-radius: 8px;
    margin-bottom: 1rem;
  }
  
  .response-box {
    background-color: white;
    padding: 1rem;
    border-radius: 8px;
  }
  
  .response-box h6 {
    color: var(--primary);
    margin-bottom: 0.75rem;
    display: flex;
    align-items: center;
    gap: 0.5rem;
  }
  
  .original-comment {
    background-color: #f8f9fa;
    padding: 1rem;
    border-radius: 8px;
    margin-bottom: 1.5rem;
  }
  
  .response-form label, .reject-reason label {
    font-weight: 500;
    margin-bottom: 0.5rem;
    display: block;
  }
  
  .response-form textarea, .reject-reason textarea {
    width: 100%;
    padding: 0.75rem;
    border: 1px solid #e0e0e0;
    border-radius: 8px;
    resize: vertical;
  }
  
  .confirmation-message {
    margin-bottom: 1rem;
    font-weight: 500;
  }
  
  .comment-preview {
    background-color: #f8f9fa;
    padding: 1rem;
    border-radius: 8px;
    margin-bottom: 1.5rem;
  }
  
  .bulk-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: 1.5rem;
  }
  
  .alert-box {
    background-color: rgba(52, 152, 219, 0.1);
    color: var(--info);
    padding: 0.75rem 1rem;
    border-radius: 8px;
    display: flex;
    align-items: center;
    gap: 0.5rem;
  }
  
  .select-all-option {
    display: flex;
    align-items: center;
    gap: 0.5rem;
    cursor: pointer;
  }
  
  .bulk-comments {
    max-height: 300px;
    overflow-y: auto;
    border: 1px solid #e0e0e0;
    border-radius: 8px;
  }
  
  .bulk-comment-item {
    display: flex;
    padding: 0.75rem;
    border-bottom: 1px solid #e0e0e0;
  }
  
  .bulk-comment-item:last-child {
    border-bottom: none;
  }
  
  .selection {
    padding-right: 0.75rem;
  }
  
  .comment-info {
    flex: 1;
  }
  
  .comment-preview {
    font-size: 0.875rem;
    color: var(--gray);
    margin-top: 0.25rem;
    white-space: nowrap;
    overflow: hidden;
    text-overflow: ellipsis;
    max-width: 400px;
  }
  
  .cancel-btn {
    background-color: #f8f9fa;
    color: var(--gray);
    border: none;
    border-radius: 30px;
    padding: 0.5rem 1.25rem;
    font-weight: 500;
    transition: var(--transition);
  }
  
  .approve-btn-lg {
    background-color: var(--success);
    color: white;
    border: none;
    border-radius: 30px;
    padding: 0.5rem 1.25rem;
    font-weight: 500;
    transition: var(--transition);
  }
  
  .reject-btn-lg {
    background-color: var(--danger);
    color: white;
    border: none;
    border-radius: 30px;
    padding: 0.5rem 1.25rem;
    font-weight: 500;
    transition: var(--transition);
  }
  
  .respond-btn-lg {
    background-color: var(--info);
    color: white;
    border: none;
    border-radius: 30px;
    padding: 0.5rem 1.25rem;
    font-weight: 500;
    transition: var(--transition);
  }
  
  .submit-btn {
    background-color: var(--info);
    color: white;
    border: none;
    border-radius: 30px;
    padding: 0.5rem 1.25rem;
    font-weight: 500;
    transition: var(--transition);
  }
  
  .cancel-btn:hover, .approve-btn-lg:hover, .reject-btn-lg:hover, .respond-btn-lg:hover, .submit-btn:hover {
    transform: translateY(-2px);
    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
  }
  
  /* Responsive Styles */
  @media (max-width: 992px) {
    .view-layout {
      grid-template-columns: 1fr;
    }
  }
  
  @media (max-width: 768px) {
    .stat-card {
      padding: 1rem;
    }
    
    .stat-icon {
      width: 40px;
      height: 40px;
      font-size: 1.25rem;
    }
    
    .stat-value {
      font-size: 1.5rem;
    }
    
    .comment-card {
      padding: 1rem;
    }
    
    .product-img {
      width: 32px;
      height: 32px;
    }
    
    .user-avatar {
      width: 32px;
      height: 32px;
    }
    
    .action-btn {
      width: 32px;
      height: 32px;
    }
  }
  
  @media (max-width: 576px) {
    .comment-header {
      flex-direction: column;
      gap: 0.75rem;
    }
    
    .comment-status {
      align-self: flex-start;
    }
    
    .user-info {
      flex-wrap: wrap;
    }
    
    .comment-date {
      margin-left: 0;
      margin-top: 0.5rem;
      width: 100%;
    }
    
    .comments-footer {
      flex-direction: column;
      gap: 1rem;
      align-items: flex-start;
    }
    
    .bulk-action-btn {
      width: 100%;
    }
  }
  </style>