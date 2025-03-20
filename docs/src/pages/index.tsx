import clsx from 'clsx';
import useDocusaurusContext from '@docusaurus/useDocusaurusContext';
import HomepageFeatures from '@site/src/components/HomepageFeatures';
import Layout from '@theme/Layout';
import Heading from '@theme/Heading';
import styles from './index.module.css';
import AnimatedGridPattern from '../components/ui/animated-grid-pattern';
import { JSX } from 'react';

function HomepageHeader() {
  const { siteConfig } = useDocusaurusContext();
  return (
    <header className={clsx('hero bg-secondary text-secondary-foreground overflow-hidden', styles.heroBanner)}>
      <AnimatedGridPattern
        numSquares={32}
        maxOpacity={0.25}
        className="inset-y-[-50%] h-[200%] skew-x-6 skew-y-6 [mask-image:radial-gradient(50vw_circle_at_center,white,transparent)]"
      />
      <div className="z-10 container">
        <Heading as="h1" className="hero__title">
          {siteConfig.title}
        </Heading>
        <p className="hero__subtitle">{siteConfig.tagline}</p>
      </div>
    </header>
  );
}

export default function Home(): JSX.Element {
  const { siteConfig } = useDocusaurusContext();
  return (
    <Layout
      title={`${siteConfig.title}`}
      description="Mathematics.NET is a C# class library that provides tools for solving advanced mathematical problems; it has support for automatic differentiation (autodiff) as well as operations commonly used in calculus, linear algebra, and differential geometry.">
      <HomepageHeader />
      <main>
        <HomepageFeatures />
      </main>
    </Layout>
  );
}
