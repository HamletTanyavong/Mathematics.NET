---
slug: road-to-release
title: Road to Release
authors: hamlettanyavong
tags: [physics, mathematics]
---

Mathematics.NET is nearing its first release, but a number of features still need to be implemented.

<!-- truncate -->

## Core Features

Here are some core features that will be included in the first release and their current status. Note that a number of basic operations are not icluded in this list.

### Numerical Types

- [x] Core types
  - [x] Real numbers
  - [x] Complex numbers
  - [x] Rational numbers

### AutoDiff

- [ ] Reverse mode autodiff
  - [x] Gradient Tapes
    - [ ] Parallelization
    - [ ] Checkpointing
  - [x] Hessian Tapes
    - [ ] Parallelization
    - [ ] Checkpointing
- [x] Forward mode autodiff
  - [x] Dual numbers
  - [x] Hyper-dual numbers

### Linear Algebra

- [x] Vectors
  - [x] Two, three, and four-element vectors
  - [x] AutoDiff vectors
  - [x] Vectors in polar, cylindrical, and spherical coordinates
- [x] Matrices
- [x] Multi-dimensional arrays

### Differential Geometry

- [x] Rank one, two, three, and four tensors
- [ ] DifGeo methods
  - [x] Index raising and lowering
  - [x] Index contractions
  - [ ] Covariant derivatives
  - [ ] Contravariant derivatives
  - [x] Tensor products
- [x] Christoffel symbols
- [x] Riemann curvature tensors
- [x] Metric tensors
- [ ] Levi-Civita symbols

### Other Features

- [ ] Solvers
  - [x] Runge-Kutta solvers
    - [ ] Solvers for differential geometry
- [ ] QR decomposition
  - [x] Gram-Schmidt process
  - [ ] Householder reflections
  - [ ] Givens rotations
