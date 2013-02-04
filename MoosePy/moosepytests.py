import unittest
import test_hours_aggregation
import test_hours_estimate
import test_reportoutput
import test_hours_filtering

# Run all test suites
if __name__ == '__main__':
    suite = unittest.TestLoader().loadTestsFromTestCase(test_hours_aggregation.TestHoursAggregation)
    suite.addTest(unittest.TestLoader().loadTestsFromTestCase(test_hours_estimate.TestHoursEstimate))
    suite.addTest(unittest.TestLoader().loadTestsFromTestCase(test_reportoutput.TestReportOutput))
    suite.addTest(unittest.TestLoader().loadTestsFromTestCase(test_hours_filtering.TestHoursFiltering))
    unittest.TextTestRunner(verbosity=1).run(suite)

