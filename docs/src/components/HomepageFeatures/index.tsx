import clsx from 'clsx';
import Heading from '@theme/Heading';
import styles from './styles.module.css';

type FeatureItem = {
  title: string;
  description: JSX.Element;
};

const FeatureList: FeatureItem[] = [
  {
    title: 'Mathematical Types',
    description: <>Use custom types for complex, real, and rational numbers as well as other types for vectors, matrices, and tensors.</>
  },
  {
    title: 'AutoDiff',
    description: <>Mathematics.NET supports first and second-order, forward and reverse-mode automatic differentiation.</>
  },
  {
    title: 'Differential Geometry',
    description: <>Use methods for common operations in differential geometry such as index raising, lowering, and contractions.</>
  }
];

function Feature({ title, description }: FeatureItem) {
  return (
    <div className={clsx('col col--4')}>
      <div className="text--center padding-horiz--md">
        <Heading as="h3">{title}</Heading>
        <p>{description}</p>
      </div>
    </div>
  );
}

export default function HomepageFeatures(): JSX.Element {
  return (
    <section className={styles.features}>
      <div className="container">
        <div className="row">
          {FeatureList.map((props, idx) => (
            <Feature key={idx} {...props} />
          ))}
        </div>
      </div>
    </section>
  );
}
