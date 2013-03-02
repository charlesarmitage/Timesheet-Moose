import unittest
from workinghourstests import test_hours_aggregation
from workinghourstests import test_hours_estimate
from workinghourstests import test_reportoutput
from workinghourstests import test_hours_filtering
from workinghourstests import test_logreader
from workinghourstests import test_hoursnormalization

# Run all test suites
if __name__ == '__main__':
    suite = unittest.TestLoader().loadTestsFromTestCase(test_hours_aggregation.TestHoursAggregation)
    suite.addTest(unittest.TestLoader().loadTestsFromTestCase(test_hours_estimate.TestHoursEstimate))
    suite.addTest(unittest.TestLoader().loadTestsFromTestCase(test_reportoutput.TestReportOutput))
    suite.addTest(unittest.TestLoader().loadTestsFromTestCase(test_hours_filtering.TestHoursFiltering))
    suite.addTest(unittest.TestLoader().loadTestsFromTestCase(test_logreader.TestLogReader))
    suite.addTest(unittest.TestLoader().loadTestsFromTestCase(test_hoursnormalization.TestNormalization))
    unittest.TextTestRunner(verbosity=1).run(suite)

