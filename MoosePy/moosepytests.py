import unittest
import test_hours_aggregation
import test_hours_estimate

# Run all test suites
if __name__ == '__main__':
    suite = unittest.TestLoader().loadTestsFromTestCase(test_hours_aggregation.TestHoursAggregation)
    suite.addTest(unittest.TestLoader().loadTestsFromTestCase(test_hours_estimate.TestHoursEstimate))
    unittest.TextTestRunner(verbosity=1).run(suite)

