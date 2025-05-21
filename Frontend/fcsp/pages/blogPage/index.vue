<template>
  <div class="blog-wrapper">
    <div class="big-image-bg"></div>
    <Header />

    <section class="container py-6">
      <!-- Introduction Section -->
      <div class="row mb-6 intro-section">
        <div class="col-12">
          <div class="intro-content text-center">
            <h2 class="intro-title blog-title-main">Custom 3D Shoe News & Articles</h2>
            <p class="intro-text blog-archive">A collection of news and stories about personalized 3D shoes</p>
          </div>
        </div>
      </div>

      <!-- Blog Grid -->
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
    label: "GUIDE",
    labelColor: "blue",
    title: "How to Design Your Own Custom 3D Shoes",
    desc: "Discover the process of creating personalized 3D shoes from concept to final product.",
    date: "February 4, 2023",
    comments: "142 comments"
  },
  {
    image: "https://i.pinimg.com/736x/33/76/f1/3376f184c88c94ccd759df1f36e2a8a7.jpg",
    label: "TECHNOLOGY",
    labelColor: "green",
    title: "Applying AI in Custom 3D Shoe Design",
    desc: "Learn how AI helps personalize every pair of shoes to fit your style and size.",
    date: "February 4, 2023",
    comments: "65 comments"
  },
  {
    image: "https://i.pinimg.com/736x/49/b3/7b/49b37b386d4eff750f3232fe9c391fbe.jpg",
    label: "TRENDS",
    labelColor: "orange",
    title: "Custom 3D Shoe Trends for 2024",
    desc: "Highlighting standout personalized 3D shoe models and the latest design trends worldwide.",
    date: "February 4, 2023",
    comments: "84 comments"
  },
  {
    image: "https://i.pinimg.com/736x/d7/4d/ca/d74dcaf34ee2446f2c49a28e0c6bdae3.jpg",
    label: "COMPARISON",
    labelColor: "purple",
    title: "Comparing Popular Online 3D Shoe Design Platforms",
    desc: "Reviewing the pros and cons of widely used 3D shoe design tools available today.",
    date: "February 4, 2023",
    comments: "29 comments"
  },
  {
    image: "https://i.pinimg.com/736x/38/2b/ac/382bac18bfbd2e6a3dc07353a53a3ad8.jpg",
    label: "REAL EXPERIENCE",
    labelColor: "teal",
    title: "Real Experience: Ordering Custom 3D Shoes Online",
    desc: "Sharing the firsthand experience of ordering and receiving personalized 3D shoes.",
    date: "February 4, 2023",
    comments: "189 comments"
  },
  {
    image: "https://i.pinimg.com/736x/de/10/54/de1054b729278c8fc75099f4e27d12c8.jpg",
    label: "REVIEWS",
    labelColor: "purple",
    title: "Top 5 Trusted Custom 3D Shoe Manufacturers",
    desc: "A list of reputable custom 3D shoe workshops and stores highly rated by customers.",
    date: "February 4, 2023",
    comments: "69 comments"
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
  background: #AAAAAA;
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
  background: #AAAAAA;
  color: #fff;
  padding: 4px 12px;
  border-radius: 12px;
  font-size: 12px;
  font-weight: bold;
  z-index: 2;
}
.label-blue { background: #AAAAAA; }
.label-green { background: #AAAAAA; }
.label-orange { background: #AAAAAA; }
.label-purple { background: #AAAAAA; }
.label-teal { background: #AAAAAA; }
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
  .blog-title-main {
    font-size: 1.5rem;
    margin-top: 60px;
  }
}
</style>