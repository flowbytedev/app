from setuptools import find_packages, setup

setup(
    name="dagster",
    packages=find_packages(exclude=["flowbyte_app_tests"]),
    install_requires=[
        "dagster",
        "dagster-cloud"
    ],
    extras_require={"dev": ["dagit", "pytest"]},
)
