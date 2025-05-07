<template>
  <div class="blog-wrapper">
    <div class="big-image-bg"></div>
    <Header />

    <section class="container py-6">
      <!-- Introduction Section -->
      <div class="row mb-6 intro-section">
        <div class="col-12">
          <div class="intro-content text-center">
            <h2 class="intro-title blog-title-main">Custom Shoe 3D News & Articles</h2>
            <p class="intro-text blog-archive">Tổng hợp tin tức & bài viết về giày tuỳ chỉnh 3D</p>
          </div>
        </div>
      </div>
      <div class="blog-grid">
        <div class="blog-card" v-for="(post, idx) in posts" :key="idx">
          <div class="blog-img-wrapper">
            <img :src="post.image" :alt="post.title" />
            <span class="blog-label" :class="'label-' + post.labelColor">{{ post.label }}</span>
          </div>
          <h2 class="blog-title">{{ post.title }}</h2>
          <p class="blog-desc">{{ post.desc }}</p>
          <div class="blog-meta">
            <span>{{ post.date }}</span>
            <span>• {{ post.comments }}</span>
          </div>
        </div>
      </div>
    </section>

    <Footer />
  </div>
</template>

<script setup>
import Header from '~/components/Header.vue';
import Footer from '~/components/Footer.vue';

const posts = [
  {
    image: "https://i.pinimg.com/736x/a9/6c/e1/a96ce1906ae0c00b82e514db4b612379.jpg",
    label: "HƯỚNG DẪN",
    labelColor: "blue",
    title: "Cách thiết kế giày 3D tuỳ chỉnh cho riêng bạn",
    desc: "Khám phá quy trình tạo ra đôi giày 3D cá nhân hoá từ ý tưởng đến sản phẩm thực tế.",
    date: "4 Tháng 2, 2023",
    comments: "0 bình luận"
  },
  {
    image: "https://i.pinimg.com/736x/33/76/f1/3376f184c88c94ccd759df1f36e2a8a7.jpg",
    label: "CÔNG NGHỆ",
    labelColor: "green",
    title: "Ứng dụng AI trong thiết kế giày 3D tuỳ chỉnh",
    desc: "Tìm hiểu cách AI giúp cá nhân hoá từng đôi giày theo phong cách và kích thước của bạn.",
    date: "4 Tháng 2, 2023",
    comments: "0 bình luận"
  },
  {
    image: "https://i.pinimg.com/736x/49/b3/7b/49b37b386d4eff750f3232fe9c391fbe.jpg",
    label: "XU HƯỚNG",
    labelColor: "orange",
    title: "Xu hướng giày 3D tuỳ chỉnh năm 2024",
    desc: "Những mẫu giày 3D cá nhân hoá nổi bật và xu hướng thiết kế mới nhất trên thế giới.",
    date: "4 Tháng 2, 2023",
    comments: "0 bình luận"
  },
  {
    image: "https://i.pinimg.com/736x/d7/4d/ca/d74dcaf34ee2446f2c49a28e0c6bdae3.jpg",
    label: "SO SÁNH",
    labelColor: "purple",
    title: "So sánh các nền tảng thiết kế giày 3D online",
    desc: "Đánh giá ưu nhược điểm của các công cụ thiết kế giày 3D phổ biến hiện nay.",
    date: "4 Tháng 2, 2023",
    comments: "0 bình luận"
  },
  {
    image: "https://i.pinimg.com/736x/38/2b/ac/382bac18bfbd2e6a3dc07353a53a3ad8.jpg",
    label: "THỰC TẾ",
    labelColor: "teal",
    title: "Trải nghiệm thực tế: Đặt giày 3D tuỳ chỉnh online",
    desc: "Chia sẻ trải nghiệm đặt và nhận đôi giày 3D cá nhân hoá đầu tiên.",
    date: "4 Tháng 2, 2023",
    comments: "0 bình luận"
  },
  {
    image: "https://i.pinimg.com/736x/de/10/54/de1054b729278c8fc75099f4e27d12c8.jpg",
    label: "ĐÁNH GIÁ",
    labelColor: "purple",
    title: "Top 5 xưởng sản xuất giày 3D tuỳ chỉnh uy tín",
    desc: "Danh sách các xưởng/cửa hàng giày 3D tuỳ chỉnh chất lượng, được đánh giá cao.",
    date: "4 Tháng 2, 2023",
    comments: "0 bình luận"
  }
];
</script>

<style scoped>
@import url('https://fonts.googleapis.com/css2?family=Poppins:wght@400;500;600;700&display=swap');

.blog-wrapper {
  background: linear-gradient(135deg, #f5f7fa 0%, #e4e9f2 100%);
  min-height: 100vh;
  font-family: 'Poppins', sans-serif;
  position: relative;
  display: flex;
  flex-direction: column;
}
.big-image-bg {
  width: 100vw;
  height: 550px;
  min-height: 250px;
  max-height: 500px;
  background: url('https://kingshoes.vn/data/upload/media/af1-2.jpg') center center/cover no-repeat;
  position: absolute;
  top: 0;
  left: 0;
  z-index: 0;
}
.container {
  position: relative;
  z-index: 1;
  color: #1e293b;
  max-width: 1300px;
  margin: 0 auto;
  flex: 1;
  padding-bottom: 60px;
  min-height: 400px;
}

.intro-section {
  padding: 60px 0 0 0;
  background: #ffffff;
  box-shadow: 0 4px 20px rgba(0, 0, 0, 0.05);
  margin-bottom: 60px;
  width: 100vw;
  position: relative;
  left: 50%;
  right: 50%;
  margin-left: -50vw;
  margin-right: -50vw;
}
.intro-content {
  margin-bottom: 40px;
}
.intro-title {
  font-size: 2.5rem;
  font-weight: 600;
  background: #007bff;
  -webkit-background-clip: text;
  background-clip: text;
  color: transparent;
  margin-bottom: 20px;
  letter-spacing: 1px;
}
.intro-text {
  font-size: 1.1rem;
  color: #000000;
  line-height: 1.8;
  max-width: 900px;
  margin: 0 auto;
}
.blog-grid {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: 32px;
}
.blog-card {
  background: #fff;
  border-radius: 16px;
  box-shadow: 0 2px 12px rgba(0,0,0,0.07);
  padding: 0 0 16px 0;
  text-align: left;
  transition: box-shadow 0.2s, transform 0.2s;
  position: relative;
  overflow: hidden;
  display: flex;
  flex-direction: column;
  min-height: 0;
}
.blog-card:hover {
  box-shadow: 0 8px 32px rgba(0,0,0,0.16);
  transform: translateY(-6px) scale(1.03);
}
.blog-img-wrapper {
  position: relative;
  border-radius: 16px 16px 0 0;
  overflow: hidden;
}
.blog-img-wrapper img {
  width: 100%;
  height: 220px;
  object-fit: cover;
  border-radius: 16px 16px 0 0;
  transition: transform 0.3s;
}
.blog-card:hover .blog-img-wrapper img {
  transform: scale(1.05);
}
.blog-label {
  position: absolute;
  top: 12px;
  right: 12px;
  background: #1976d2;
  color: #fff;
  padding: 4px 12px;
  border-radius: 12px;
  font-size: 12px;
  font-weight: bold;
  z-index: 2;
}
.label-blue { background: #1976d2; }
.label-green { background: #1976d2; }
.label-orange { background: #1976d2; }
.label-purple { background: #1976d2; }
.label-teal { background: #1976d2; }
.blog-title {
  font-size: 20px;
  margin: 16px 16px 8px 16px;
  font-weight: 600;
  line-height: 1.3;
  min-height: 48px;
}
.blog-desc {
  color: #666;
  margin: 0 16px 16px 16px;
  font-size: 15px;
  flex: 1;
}
.blog-meta {
  color: #aaa;
  font-size: 13px;
  margin: 0 16px;
  display: flex;
  justify-content: space-between;
  align-items: center;
  border-top: 1px solid #f0f0f0;
  padding-top: 8px;
}
@media (max-width: 1024px) {
  .blog-grid {
    grid-template-columns: repeat(2, 1fr);
  }
}
@media (max-width: 700px) {
  .blog-grid {
    grid-template-columns: 1fr;
  }
  .blog-header-bg {
    height: 120px;
  }
  .blog-title-main {
    font-size: 1.5rem;
    margin-top: 60px;
  }
}
</style>